using System;
using Gtk;
using info.lundin.math;

public partial class MainWindow: Gtk.Window
{
    public MainWindow()
        : base(Gtk.WindowType.Toplevel)
    {
        Build();

    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnAboutActionActivated(object sender, EventArgs e)
    {
        var dialog = new AboutDialog();
        dialog.Version = "0.0.1";
        dialog.Title = "Nate's Calculator";
        dialog.Website = "https://www.GitHub.com/ndaljr/Calculator";
        dialog.Comments = "This is a basic calculator that allows for expressions to be evaluated." +
        " You can email me at ndaljr@gmail.com if you notice any bugs.\n"
        + "\nCredit goes to the author of the info.lundin.math dll. This calculator uses the parser provided by the" +
        " aforementioned package.";
        dialog.Authors = new string[] { "Nathaniel D. Alcedo Jr" };
        dialog.ShowAll();
        dialog.Run();
        dialog.Destroy();
    }

    protected void OnCloseActionActivated(object sender, EventArgs e)
    {
        Application.Quit();
    }

    protected void OnButtonClearClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Clear();
    }

    protected void OnButtonOneClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += "1";
    }

    protected void OnButtonTwoClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += "2";
    }

    protected void OnButtonThreeClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += "3";
    }

    protected void OnButtonDivideClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += "/";
    }

    protected void OnButtonFourClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += "4";
    }

    protected void OnButtonFiveClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += "5";
    }

    protected void OnButtonSixClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += "6";
    }

    protected void OnButtonSevenClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += "7";
    }

    protected void OnButtonEightClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += "8";
    }

    protected void OnButtonNineClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += "9";
    }

    protected void OnButtonMultiplyClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += " * ";
    }

    protected void OnButtonMinusClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += " - ";
    }

    protected void OnButtonZeroClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += "0";
    }

    protected void OnButtonDecimalClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += ".";
    }

    protected void OnButtonAddClicked(object sender, EventArgs e)
    {
        _Result.Buffer.Text += " + ";
    }

    protected void OnButtonCalculateClicked(object sender, EventArgs e)
    {
        
        CheckExpressionFormat();
    }

    [GLib.ConnectBefore]
    protected void OnKeyPressEvent(object source, KeyPressEventArgs args)
    {
        if (args.Event.Key == Gdk.Key.KP_Enter || args.Event.Key == Gdk.Key.Return)
        {
            CheckExpressionFormat();
        }
    }


    private void CheckExpressionFormat()
    {
        string currentExpression = _Result.Buffer.Text.TrimEnd();
        currentExpression = currentExpression.Replace("\n", " ");
        string[] expressionsComponents = currentExpression.Split(new char[]{ ' ' });
        if (expressionsComponents.Length == 1)
        {
            CalculateFunction(currentExpression);
        }
        if (expressionsComponents.Length > 1)
        {
            foreach (var component in expressionsComponents)
            {
                if (component == expressionsComponents[0])
                {
                    if (CheckIfExpressionContainsOperator(component[component.Length - 1]))
                    {
                        continue;
                    }
                }
                else if (CheckIfExpressionContainsOperator(component[0]))
                {
                    continue;
                }
                else
                {
                    CalculateFunction(currentExpression);
                    _Result.Buffer.Text = expressionsComponents[expressionsComponents.Length - 1];
                    return;
                }
            }

            CalculateFunction(currentExpression);
        }
    }

    private void CalculateFunction(string expression)
    {
        var parser = new ExpressionParser();
        double result;
        if (expression == string.Empty)
        {
            return;
        }
        try
        {
            result = parser.Parse(expression);
            PrintResult(result);
        }
        catch (ParserException ex)
        {
            ThrowErrorDialog();
            return;
        }

    }

    void PrintResult(double result)
    {
        _Result.Buffer.Clear();
        _Result.Buffer.Text += result.ToString();
    }

    private void ThrowErrorDialog()
    {
        var errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close,
                              "Expression format Error. Try again");
        errorDialog.ShowAll();
        errorDialog.Run();
        errorDialog.Destroy();
    }

    private bool CheckIfExpressionContainsOperator(char character)
    {
        if (character == '+' || character == '-' || character == '/' || character == '*')
        {
            return true;
        }
        return false;
    }

   
}
