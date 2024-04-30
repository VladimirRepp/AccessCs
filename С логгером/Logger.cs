using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Confectionery
{
    // Logger.GetInstance().SetViewPort(toolStripLabel_StatusLoger);
    // Logger.GetInstance().SetTypeViewPort(EViewPortLogger.ToolStripLabel);

    public class Logger
    {
        private ToolStripLabel _toolStripLabel;
        private EViewPortLogger _typeViewPort;

        private static Logger _instance;

        private Logger()
        {
            _toolStripLabel = null;
            _typeViewPort = EViewPortLogger.MessageBox;
        }

        public static Logger GetInstance()
        {
            if (_instance == null)
                _instance = new Logger();
            return _instance;
        }

        public void SetViewPort(ToolStripLabel label)
        {
            _typeViewPort = EViewPortLogger.ToolStripLabel;
            _toolStripLabel = label;
        }

        public void SetTypeViewPort(EViewPortLogger typeViewPort)
        {
            _typeViewPort = typeViewPort;
        }

        public void Notify(string message)
        {
            switch (_typeViewPort)
            {
                case EViewPortLogger.MessageBox:
                    MessageBox.Show(message, "Внимание!");
                    break;

                case EViewPortLogger.ToolStripLabel:
                    if (_toolStripLabel != null)
                        _toolStripLabel.Text = message;
                    else
                        throw new Exception("Строка статуса не установлена в логере для текущего окна!");
                    break;
            }
        }
    }

    public enum EViewPortLogger
    {
        Non,
        MessageBox,
        ToolStripLabel,
        TextBox
    }
}
