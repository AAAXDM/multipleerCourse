﻿using Microsoft.Extensions.DependencyInjection;
using Server;

int sleepTime = 15;

var serviceProvider = Container.Configure();
var server = serviceProvider.GetRequiredService<NetworkServer>();
ServerDbContext db = new();

server.Start();

while (true)
{
    server.PollEvents();
    Thread.Sleep(sleepTime);  
}

