using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentTemplate.Interfaces;

public interface IBaseElement
{

    public void OnHover();

    public void OnClick();

    public void OnDoubleClick();

    public void OnRightClick();

}