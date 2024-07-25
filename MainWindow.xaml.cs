using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<TaskButton> taskList = new List<TaskButton>();

        private static TaskButton activeTaskButton;
        private Timer timer;
        private static int timerCount;


        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = true;
            this.addTask("Обед");
            this.timer = new Timer(1000);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.addTask(Input.Text);
        }


        public void addTask(string name)
        {
            TaskButton taskButton = new TaskButton(taskList.Count + 1, name);
            Button newButton = this.createButton(taskButton);
            taskButton.setButtonUI(newButton);
            taskList.Add(taskButton);
        }


        public Button createButton(TaskButton taskButton)
        {
            Button button = new Button();

            button.Click += NewButton_Click;
            button.PreviewMouseRightButtonDown += NewButton_RigthClick;

            button.Name = "task_" + taskButton.getId().ToString();

            button.Content = taskButton.getName();

            myStackPanel.Children.Add(button);

            return button;
        }


        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            string buttonName = button.Name;

            int id = int.Parse(buttonName.Split('_')[1]) - 1;

            activeTaskButton = taskList[id];

/*            if (this.timer.Enabled == false)
            {
                this.timer.Start();
                // Hook up the Elapsed event for the timer. 
                this.timer.Elapsed += OnTimedEvent;
                this.timer.AutoReset = true;
                this.timer.Enabled = true;
            }*/

            if (this.timer.Enabled == false)
            {
                // Hook up the Elapsed event for the timer. 
                this.timer.Elapsed += OnTimedEvent; // Убедитесь, что здесь нет 'static'
                this.timer.AutoReset = true;
                this.timer.Enabled = true;
            }


        }


        private void NewButton_RigthClick(object sender, RoutedEventArgs e)
        {

            addTask("subtask1");

        }

        // Замените строку
        // private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            // Вызываем метод обновления в UI-потоке
            Application.Current.Dispatcher.Invoke(() =>
            {
                timerCount++;

                int activeTaskButtonTime = activeTaskButton.getTime() + 1;

                activeTaskButton.setTime(activeTaskButtonTime);

                activeTaskButton.getButtonUI().Content = activeTaskButton.getName() + " | " + SecondsToHMS(activeTaskButtonTime);

/*                if (timerCount == 2 * 60 * 60)
                {
                    MessageBox.Show("Проставь трудозатраты");
                }

                if (timerCount == 4 * 60 * 55)
                {
                    MessageBox.Show("Проставь трудозатраты и иди на обед");
                }

                if (timerCount == 6 * 60 * 60)
                {
                    MessageBox.Show("Проставь трудозатраты отдохни");
                }

                if (timerCount == 8 * 60 * 60)
                {
                    MessageBox.Show("Проставь трудозатраты и иди спортом заниматься");
                }*/

                timeLeftLabel.Content = "Пройдено Времени: " + SecondsToHMS(timerCount);

            });
        }


        static string SecondsToHMS(int seconds)
        {
            int hours = seconds / 3600;
            int minutes = (seconds % 3600) / 60;
            seconds = seconds % 60;


            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        }


        override protected void OnClosed(EventArgs e)
        {
            using (StreamWriter outputFile = new StreamWriter("трудозатраты.txt", true))
            {
                outputFile.WriteLine(DateTime.Now);

                for (int i = 0; i < taskList.Count; i++)
                {
                    TaskButton taskButton = taskList[i];

                    outputFile.WriteLine(taskButton.getName() + " | " + SecondsToHMS(taskButton.getTime()));
                }

                outputFile.WriteLine("");
            }
            FileStream fileStream = File.OpenWrite("трудозатраты.txt");
        }
    }
}
