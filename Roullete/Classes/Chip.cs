using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Roullete
{
   sealed class Chip
    {
        
        public int valueDigit { get; set; }
        public PictureBox chipBox;
        public Chip(int _valueDigit, PictureBox _chipBox, Image _image)
        {
            chipBox = _chipBox;
            chipBox.BackgroundImage = _image;
            valueDigit = _valueDigit;
        }
    }
}
  