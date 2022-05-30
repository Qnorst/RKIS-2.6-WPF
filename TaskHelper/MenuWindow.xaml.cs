using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskHelper.Models;

namespace TaskHelper
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : System.Windows.Window
    {
        
        public MenuWindow()
        {
            InitializeComponent();
            LoadData();
            User currentUser;                        
            currentUser = Helper.userSession;
            DataContext = currentUser;
           
        }
        private void LoadData()
        {
            UserTasksDGrid.ItemsSource = Helper.db.Tasks.Include(q => q.StatusTask).Include(w => w.Creator).Include(a => a.Acceptor).Where(x => x.CreatorId == Helper.userSession.UserId).ToList();
            ClosedTasksDGrid.ItemsSource = Helper.db.Tasks.Include(q => q.StatusTask).Include(w => w.Creator).Include(a => a.Acceptor).Where(x => x.AcceptorId == Helper.userSession.UserId).Where(x => x.StatusTaskId == 3).ToList();
            OpenTasksDGrid.ItemsSource = Helper.db.Tasks.Include(q => q.StatusTask).Include(w => w.Creator).Include(a => a.Acceptor).Where(x => x.StatusTaskId == 1).ToList();
            TakenTasksDGrid.ItemsSource = Helper.db.Tasks.Include(q => q.StatusTask).Include(w => w.Creator).Include(a => a.Acceptor).
            Where(x => x.AcceptorId == Helper.userSession.UserId).Where(x => x.CreatorId != Helper.userSession.UserId).Where(x => x.StatusTaskId < 3).ToList();
            UsersDGrid.ItemsSource = Helper.db.Users.ToList();
        }
        private void UsersDGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = UsersDGrid.SelectedItem as User;
            if (user != null)
            {
                Helper.db.SaveChanges();
                LoadData();
            }
        }
        private void UsersTaskSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = Helper.db.Users.FirstOrDefault(q => q.Name == SearchBox.Text.Trim());
            if (user != null)
            {
                Models.Task task = Helper.db.Tasks.FirstOrDefault(q => q.CreatorId == user.UserId);
                new TakeTaskWindow(task).Show();
                this.Close();
            }


        }
        private void OpenTasksDGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Models.Task selectedTask = OpenTasksDGrid.SelectedItem as Models.Task;

            Helper.task = selectedTask;

            if (selectedTask != null)
            {
                new TakeTaskWindow(selectedTask).Show();
                this.Close();
            }

        }
        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        private void ChangeTaskStatusBtn_Click(object sender, RoutedEventArgs e)
        {
            Models.Task selectedTask = UserTasksDGrid.SelectedItem as Models.Task;

            Models.Task task = Helper.db.Tasks.First(q => q.TaskId == selectedTask.TaskId);

            if (selectedTask.StatusTaskId == 1 || selectedTask.StatusTaskId == 2)
            {
                task.StatusTaskId++;
                Helper.db.SaveChanges();
            }
            else if (selectedTask.StatusTaskId == 3)
            {
                task.StatusTaskId = 1;
                Helper.db.SaveChanges();
            }

            LoadData();
        }

        private void CreateTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            new TaskCreateWindow().Show();
            this.Close();
            SaveFileDialog saveFile = new SaveFileDialog();
        }

        private void CreateReportBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = "report";
            saveFile.Filter = "Excel files (.xlsx)|*.xlsx|Word files (.docx)|*.docx|Pdf files (.pdf)|*.pdf";
            List<Models.Task> tasks = Helper.db.Tasks.ToList();
            if (saveFile.ShowDialog() == true)
            {
                switch (saveFile.FilterIndex)
                {
                    case 1:
                        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                        Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add();
                        Microsoft.Office.Interop.Excel.Worksheet worksheet = app.Worksheets[1];
                        worksheet.Name = "Задачи";
                        worksheet.Range["A1"].Value = "Номер";
                        worksheet.Range["B1"].Value = "Название";
                        worksheet.Range["C1"].Value = "Описание";
                        worksheet.Range["D1"].Value = "Дата публикации";
                        worksheet.Range["E1"].Value = "Создатель";
                        worksheet.Range["F1"].Value = "Исполнитель";
                        worksheet.Range["G1"].Value = "Статус";

                        for (int i = 0; i < tasks.Count; i++)
                        {
                            if (tasks[i] != null)
                            {
                                worksheet.Range[$"A{i + 2}"].Value = tasks[i].TaskId;
                                worksheet.Range[$"B{i + 2}"].Value = tasks[i].Name;
                                worksheet.Range[$"C{i + 2}"].Value = tasks[i].Describtion;
                                worksheet.Range[$"D{i + 2}"].Value = tasks[i].PublicDate;
                                worksheet.Range[$"E{i + 2}"].Value = tasks[i].Creator.Login;
                                worksheet.Range[$"F{i + 2}"].Value = tasks[i].Acceptor.Login;
                                worksheet.Range[$"G{i + 2}"].Value = tasks[i].StatusTask.Name;
                            }
                        }
                        workbook.SaveAs(saveFile.FileName);
                        workbook.Close();
                        app.Quit();
                        break;
                    case 2:
                    case 3:
                        Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                        Microsoft.Office.Interop.Word.Document document = wordApp.Documents.Add();
                        Microsoft.Office.Interop.Word.Paragraph paragraph = document.Paragraphs.Add();
                        Microsoft.Office.Interop.Word.Range range = paragraph.Range;
                        Microsoft.Office.Interop.Word.Table table = document.Tables.Add(range, tasks.Count + 1, 7);
                        Microsoft.Office.Interop.Word.Range cellRange;

                        cellRange = table.Cell(1, 1).Range;
                        cellRange.Text = "Номер";
                        cellRange = table.Cell(1, 2).Range;
                        cellRange.Text = "Название";
                        cellRange = table.Cell(1, 3).Range;
                        cellRange.Text = "Описание";
                        cellRange = table.Cell(1, 4).Range;
                        cellRange.Text = "Дата публикации";
                        cellRange = table.Cell(1, 5).Range;
                        cellRange.Text = "Создатель";
                        cellRange = table.Cell(1, 6).Range;
                        cellRange.Text = "Исполнитель";
                        cellRange = table.Cell(1, 7).Range;
                        cellRange.Text = "Статус";

                        for (int i = 0; i < tasks.Count; i++)
                        {                        
                                cellRange = table.Cell(i + 2, 1).Range;
                                cellRange.Text = tasks[i].TaskId.ToString();
                                cellRange = table.Cell(i + 2, 2).Range;
                                cellRange.Text = tasks[i].Name;
                                cellRange = table.Cell(i + 2, 3).Range;
                                cellRange.Text = tasks[i].Describtion; ;
                                cellRange = table.Cell(i + 2, 4).Range;
                                cellRange.Text = tasks[i].PublicDate.ToString();
                                cellRange = table.Cell(i + 2, 5).Range;
                                cellRange.Text = tasks[i].Creator.Login;
                                cellRange = table.Cell(i + 2, 6).Range;
                                cellRange.Text = tasks[i].Acceptor.Login;
                                cellRange = table.Cell(i + 2, 7).Range;
                                cellRange.Text = tasks[i].StatusTask.Name;
                            
                        }
                        if (saveFile.FilterIndex == 2)
                        {
                            document.SaveAs2(saveFile.FileName);
                        }
                        else
                        {
                            document.SaveAs2(saveFile.FileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                        }
                        break;

                }
            }
        }
    }
}
