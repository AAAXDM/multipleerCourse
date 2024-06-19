using Srver;

int sleepTime = 15;

NetworkServer server = new();
server.Start();

while (true)
{
    server.PollEvents();
    Thread.Sleep(sleepTime);  
}

