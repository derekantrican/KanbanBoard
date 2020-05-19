using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using KanbanBoard.Models;
using LiteDB;
using System.Linq;

namespace KanbanBoard
{
    public class MainWindow : Window
    {
        LiteDatabase db = Database.Connect();
        ILiteCollection<BoardModel> boardCollection;
        ILiteCollection<ColumnModel> columnCollection;
        MenuItem menuItemAdd;
        ObjectId currentBoardId;
        Button buttonAddColumn;
        StackPanel stackPanelColumn;
        MenuItem menuItemBoardEdit;
        MenuItem menuItemBoardDelete;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.WindowState = WindowState.Normal;
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            boardCollection = db.GetCollection<BoardModel>();
            columnCollection = db.GetCollection<ColumnModel>();

            menuItemAdd = new MenuItem { Header = "" };
            menuItemAdd.Click += async (sender, e) =>
            {
                var addBoard = new AddBoardWindow();
                var result = await addBoard.ShowDialog<BoardModel>(this);
                if (result != null)
                {
                    currentBoardId = boardCollection.Insert(result);
                    RefreshMenuBoard();
                }
            };

            menuItemBoardEdit = this.FindControl<MenuItem>("menuItemBoardEdit");
            menuItemBoardEdit.Click += async (sender, e) =>
            {
                var board = boardCollection.FindById(currentBoardId);
                var addBoard = new AddBoardWindow(ref board);
                var result = await addBoard.ShowDialog<BoardModel>(this);
                if (result != null)
                {
                    boardCollection.Update(result);
                    RefreshMenuBoard();
                }
            };

            menuItemBoardDelete = this.FindControl<MenuItem>("menuItemBoardDelete");
            menuItemBoardDelete.Click += async (sender, e) =>
            {
                var messageBox = new MessageBoxWindow("Confirmation", "Are you sure you want to delete this board?");
                var result = await messageBox.ShowDialog<string>(this);
                if (result == "Yes")
                {
                    boardCollection.Delete(currentBoardId);
                    currentBoardId = null;
                    RefreshMenuBoard();
                    stackPanelColumn.Children.Clear();
                    buttonAddColumn.IsVisible = false;
                }
            };

            buttonAddColumn = this.FindControl<Button>("buttonAddColumn");
            buttonAddColumn.Click += async (sender, e) =>
            {
                var addColumn = new AddColumnWindow();
                var result = await addColumn.ShowDialog<ColumnModel>(this);
                if (result != null)
                {
                    result.BoardModelId = currentBoardId;
                    columnCollection.Insert(result);
                    RefreshColumn();
                }
            };

            stackPanelColumn = this.FindControl<StackPanel>("stackPanelColumn");

            RefreshMenuBoard();   
        }

        private void RefreshMenuBoard()
        {
            var menuBoard = this.FindControl<Menu>("menuBoard");
            var menuItems = boardCollection.FindAll()
                .OrderBy(x => x.Id)
                .Select(board =>
                {
                    var menuItem = new MenuItem { Name = board.Id.ToString(), Header = board.Name };
                    menuItem.Click += menuBoardItemClick;
                    if (currentBoardId != null && currentBoardId == board.Id)
                    {
                        menuItem.Classes.Add("selected");
                        buttonAddColumn.IsVisible = true;
                        menuItemBoardEdit.IsVisible = true;
                        menuItemBoardDelete.IsVisible = true;
                        RefreshColumn();
                    }
                    else
                    {
                        stackPanelColumn.Children.Clear();
                        buttonAddColumn.IsVisible = false;
                        menuItemBoardEdit.IsVisible = false;
                        menuItemBoardDelete.IsVisible = false;
                    }

                    return menuItem;
                })
                .ToList();

            menuItems.Add(menuItemAdd);
            menuBoard.Items = menuItems;
        }

        private void menuBoardItemClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem source)) return;

            foreach (MenuItem menuItem in source.GetLogicalParent<Menu>().GetLogicalChildren())
            {
                menuItem.Classes.Clear();
            }

            source.Classes.Add("selected");
            currentBoardId = new ObjectId(source.Name);
            buttonAddColumn.IsVisible = true;
            menuItemBoardEdit.IsVisible = true;
            menuItemBoardDelete.IsVisible = true;
            RefreshColumn();
        }

        private void RefreshColumn()
        {
            stackPanelColumn.Children.Clear();
            columnCollection.EnsureIndex(x => x.BoardModelId);
            stackPanelColumn.Children.AddRange(columnCollection
                .Find(x => x.BoardModelId == currentBoardId)
                .OrderBy(x => x.Id)
                .Select(x => new ColumnUserControl(ref db, ref x, (y) => stackPanelColumn.Children.Remove(y))));
        }
    }
}
