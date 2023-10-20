using System.Windows.Controls;

namespace Infections.Models;

internal interface IDrawable
{
    void Draw(ref Canvas canvas);
}