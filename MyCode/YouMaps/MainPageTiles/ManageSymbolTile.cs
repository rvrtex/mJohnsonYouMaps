using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouMaps.MainPafeTiles
{
    class ManageSymbolTile : IFrontPageTile
    {
        string image;
        string title;
        string subtitle;
        public string Image
        {
            
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }

        public string Title
        {
            get
            {
                return title;    
            }
            set
            {
                title = value;
            }
        }

        public string Subtitle
        {
            get
            {
                return subtitle;
            }
            set
            {
                subtitle = value;
            }
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
