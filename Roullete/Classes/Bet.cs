using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Roullete
{
    sealed class Bet
    {
        public Chip chip;
        public PictureBox currentBox;

        public Bet(int _valueDigit, PictureBox _currentBox, PictureBox _chip, Image _image)
        {
            chip = new Chip(_valueDigit, _chip, _image);
            currentBox = _currentBox;  
        }
 }
}
