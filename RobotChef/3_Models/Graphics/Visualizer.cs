using RobotChefProject._3_Models.FileHandling;
using Spectre.Console;
using System.Text;

namespace RobotChefProject._3_Models.Graphics
{
    public class Visualizer
    {
        #region FileHandler
        private readonly FileHandler _fileHandler;
        #endregion
        #region Variables
        private List<string> _history = [];
        #endregion
        #region Constructor
        public Visualizer(FileHandler fileHandler) 
        {
            this._fileHandler = fileHandler;
            Console.CursorVisible = false;
            CleanUp();
        }
        #endregion
        #region Public Methods
        public void SetupUI()
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

        public void Display(string message)
        {
            CleanUp();
            _history.Add($"{message}");

            AnsiConsole.MarkupLine($"[green]{message}[/]");
        }
        public void Display(string imageName, string message)
        {
            CleanUp();
            _history.Add($"{message}");

            Layout layout = new("Main");
            Layout imageLayout = new("Image") { Size = 49 };
            Layout messageLayout = new("Message") { Size = 1 };
            layout.SplitRows(
                imageLayout,
                messageLayout
            );
            layout["Image"].Update(
                new Align(
                    new CanvasImage(_fileHandler.GetImagePath(imageName)),
                    HorizontalAlignment.Center,
                    VerticalAlignment.Top
                )
            );
            // message
            layout["Message"].Update(
                new Align(
                    new Markup($"RobotChef: \"{message}\""),
                    HorizontalAlignment.Center,
                    VerticalAlignment.Top
                )
            );
            AnsiConsole.Write(layout);
        }
        public void Display(string imageName, string message, int timems)
        {
            CleanUp();
            _history.Add($"{message}");

            Layout layout = new("Main");
            Layout imageLayout = new("Image"){ Size = 49 };
            Layout messageLayout = new("Message"){ Size = 1 };
            layout.SplitRows(
                imageLayout,
                messageLayout
            );
            layout["Image"].Update(
                new Align(
                    new CanvasImage(_fileHandler.GetImagePath(imageName)),
                    HorizontalAlignment.Center,
                    VerticalAlignment.Top
                )
            );
            // message with spinner
            AnsiConsole.Live(layout)
                .Start(ctx =>
                {
                    for (var i = 0; i < timems; i++)
                    {
                        layout["Message"].Update(
                            new Align(
                                new Markup($"[red]{Spinner.Known.Ascii.Frames[i % Spinner.Known.Ascii.Frames.Count]}[/] [yellow]{message}[/]"),
                                HorizontalAlignment.Center,
                                VerticalAlignment.Top
                            )
                        );

                        ctx.Refresh();
                    }
                    layout["Message"].Update(
                            new Align(
                                new Markup($"[green]✓[/] [yellow]{message}[/]"),
                                HorizontalAlignment.Center,
                                VerticalAlignment.Top
                            )
                        );
                    ctx.Refresh();
                    Thread.Sleep(200);
                });
        }

        public void DisplayHistory()
        {
            CleanUp();

            var table = new Table();
            table.AddColumn("History");
            foreach (var msg in _history)
                table.AddRow(msg);
            AnsiConsole.Write(table);
        }
        #endregion
        #region Private Methods
        private void CleanUp()
        {
            Console.SetCursorPosition(0, 0);
            Console.Clear();
        }
        #endregion
    }
}
