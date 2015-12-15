using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Psns.Common.Mvc.ViewBuilding.Menu;

namespace WMS.Infrastructure
{
public class ApplicationMenu : IMenuItem
    {
        public MenuNode RootNode
        {
            get
            {
                return new MenuNode
                {
                    Text = "Demo Menu",
                    Children = new Collection<MenuNode> 
                    {
                        new MenuNode
                        {
                            Text = "Google",
                            Title = "Link to Google",
                            Url = "https://www.google.com"
                        }
                    }
                };
            }
        }
    }

    public class ContextMenu : IContextMenuWithDropDowns
    {
        public string Title
        {
            get { return "Edit Me in Infrastructure.ContextMenu";  }
        }

        public IEnumerable<IMenuItem> MenuItems
        {
            get { yield return new ApplicationMenu(); }
        }
    }
}
