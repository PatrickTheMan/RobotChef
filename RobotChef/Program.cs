using Newtonsoft.Json;
using RobotChefProject._2_Domain;
using RobotChefProject._3_Models;
using Spectre.Console;
using System.Runtime.Intrinsics.Arm;
using System.Text;

public class Program
{
    private static readonly RobotChef _robotChef = new();

    static void Main(string[] args)
    {
        SetupUI();
        _robotChef.Start();
    }

    private static void SetupUI()
    {
        #region Setting the console
        Console.Title = "RobotChefProject";
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.Unicode;
        #endregion

        #region Screen Size Check
        //Check for if the console has the right size
        do
        {
            if (Console.WindowHeight < 50 || Console.WindowWidth < 101)
            {
                Console.SetCursorPosition(0, Console.WindowHeight / 2 - 1);
                AnsiConsole.Write(new Align(new Panel(new Markup("[red]Wrong window size, full screen your console and continue[/]")), HorizontalAlignment.Center, VerticalAlignment.Middle));
            }
            else
                break;
            Thread.Sleep(1000);
        }
        while (true);
        #endregion

        //Blocks scrolling, works only on Windows, probably only on 10
        Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
    }
}