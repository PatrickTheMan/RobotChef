using RobotChefProject._3_Models;
using RobotChefProject._3_Models.FileHandling;
using RobotChefProject._3_Models.Graphics;
using Spectre.Console;
using System.Text;

public class Program
{
    private static readonly FileHandler _fileHandler = new();
    private static readonly Visualizer _visualizer = new(_fileHandler);
    private static readonly RobotChef _robotChef = new(_fileHandler, _visualizer);

    static void Main(string[] args)
    {
        _visualizer.SetupUI();
        _robotChef.Start();
        _robotChef.History();
    }
    
}