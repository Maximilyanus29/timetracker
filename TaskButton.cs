using System.Windows.Controls;

namespace WpfApp2
{
    public class TaskButton
    {
        private int id;
        private string name;
        private int time = 0;
        private Button ButtonUI;

        private static System.Timers.Timer timer;

        public TaskButton(int id, string name) { 
            this.id = id;
            this.name = name;
        }

        public int getId()
        {
            return this.id;
        }

        public void setId(int value)
        {
            this.id = value;
        }


        public string getName()
        {
            return this.name;
        }

        public void setName(string value)
        {
            this.name = value;
        }


        public int getTime()
        {
            return this.time;
        }

        public void setTime(int value)
        {
            this.time = value;
        }


        public Button getButtonUI()
        {
            return this.ButtonUI;
        }

        public void setButtonUI(Button value)
        {
            this.ButtonUI = value;
        }

    }
}
