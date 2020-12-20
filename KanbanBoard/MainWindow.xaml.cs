using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using KanbanBoard.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KanbanBoard
{
    public class MainWindow : Window
    {
        MenuItem menuItemAdd;
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

            Common.DeserializeBoards();

            menuItemAdd = new MenuItem { Header = "" };
            menuItemAdd.Click += async (sender, e) =>
            {
                var addBoard = new AddBoardWindow();
                var result = await addBoard.ShowDialog<BoardModel>(this);
                if (result != null)
                {
                    Common.Boards.Add(result);
                    Common.CurrentBoard = result;
                    RefreshMenuBoard();
                }
            };

            menuItemBoardEdit = this.FindControl<MenuItem>("menuItemBoardEdit");
            menuItemBoardEdit.Click += async (sender, e) =>
            {
                var addBoard = new AddBoardWindow(ref Common.CurrentBoard);
                var result = await addBoard.ShowDialog<BoardModel>(this);
                if (result != null)
                {
                    Common.Boards.Insert(Common.Boards.IndexOf(Common.CurrentBoard), result);
                    Common.SerializeBoards();

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
                    Common.Boards.Remove(Common.CurrentBoard);
                    Common.SerializeBoards();

                    Common.CurrentBoard = null;
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
                    Common.CurrentBoard.Columns.Add(result);
                    Common.SerializeBoards();

                    RefreshColumn();
                }
            };

            stackPanelColumn = this.FindControl<StackPanel>("stackPanelColumn");

            RefreshMenuBoard();   
        } 

        private void RefreshMenuBoard()
        {
            var menuBoard = this.FindControl<Menu>("menuBoard");
            var menuItems = Common.Boards
                .Select(board =>
                {
                    var menuItem = new MenuItem { Name = board.Id.ToString(), Header = board.Name };
                    menuItem.Click += menuBoardItemClick;
                    if (Common.CurrentBoard != null && Common.CurrentBoard.Id == board.Id)
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
            Common.CurrentBoard = Common.Boards.FirstOrDefault(b => b.Id == new Guid(source.Name));
            buttonAddColumn.IsVisible = true;
            menuItemBoardEdit.IsVisible = true;
            menuItemBoardDelete.IsVisible = true;
            RefreshColumn();
        }

        private void RefreshColumn()
        {
            stackPanelColumn.Children.Clear();
            stackPanelColumn.Children.AddRange(Common.CurrentBoard.Columns
                .Select(x => new ColumnUserControl(x, (y) => stackPanelColumn.Children.Remove(y))));
        }
    }
}
