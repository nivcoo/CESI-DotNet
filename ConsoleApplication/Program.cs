using ConsoleApplication.Views;

using var mutex = new Mutex(false, "easysave Application");
bool isAnotherInstanceOpen = !mutex.WaitOne(TimeSpan.Zero);
if (isAnotherInstanceOpen)
{
    Console.WriteLine("Only one instance of this app is allowed.");
    return;
}
var main = new HomeView();

