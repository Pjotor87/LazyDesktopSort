using System;
using System.Text;
using System.Windows.Forms;

namespace LazyDesktopSort
{
    public class ErrorHandler
    {
        public StringBuilder ErrorBuilder { get; set; }

        public ErrorHandler()
        {
            this.ErrorBuilder = new StringBuilder();
        }

        public void Add(string error)
        {
            if (!string.IsNullOrEmpty(error))
                this.ErrorBuilder.AppendFormat("{0}{1}", error, Environment.NewLine);
        }

        public void DisplayErrors()
        {
            if (ErrorBuilder.Length > 0)
                MessageBox.Show($"The following errors were encountered{Environment.NewLine}-------------------------------------{Environment.NewLine}{this.ErrorBuilder.ToString()}");
        }
    }
}
