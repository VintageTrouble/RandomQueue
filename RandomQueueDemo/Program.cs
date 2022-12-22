using RandomQueue;

using RandomQueueDemo;

var users = new User[]
{
    new User { Name = "User1", Id = 1 },
    new User { Name = "User2", Id = 2 },
    new User { Name = "User3", Id = 3 },
    new User { Name = "User4", Id = 4 },
    new User { Name = "User5", Id = 5 },
};

var randomQueue = new RandomQueue<User>(users);

var flag = true;
while(flag)
{
    Console.WriteLine("<1> Next\n<2> Prev\n<0> Exit");
    var action = Console.ReadKey().Key;

    switch (action)
    {
        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:
            Console.WriteLine(randomQueue.Next());
            break;
        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:
            if (randomQueue.CanGetPrev)
                Console.WriteLine(randomQueue.Prev());
            else
                Console.WriteLine("Can't get prev");
            break;
        case ConsoleKey.D0:
        case ConsoleKey.NumPad0:
            flag = false;
            break;
        default:
            break;
    }
}