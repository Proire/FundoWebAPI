using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {

        var config = AppConfiguration();
        RabbitConsumer(config);   
    }

    private static IConfiguration AppConfiguration()
    {
        var configuration = new ConfigurationBuilder().SetBasePath("C:\\Users\\proir\\source\\repos\\FundooWebAPI\\ConsumeApp").AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        return configuration.Build();
    }

    private static void RabbitConsumer(IConfiguration config)
    {
        var factory = new ConnectionFactory { HostName = config["RabbitMQ:HostName"] };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: config["RabbitMQ:QueueName"],
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Note Received {message}");
            SendEmail(message);
        };

        channel.BasicConsume(queue: config["RabbitMQ:QueueName"],
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    private static void SendEmail(string reciever)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("proiresadah12@gmail.com"));
        email.To.Add(MailboxAddress.Parse(reciever));
        email.Subject = "User Registration Status";
        email.Body = new TextPart(TextFormat.Html) { Text = "Successful Registration. Thank you for registering." };

        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("proiresadah12@gmail.com", Environment.GetEnvironmentVariable("MailPassword"));
        smtp.Send(email);
        smtp.Disconnect(true);

        Console.WriteLine($" [x] Email sent to {reciever}");
    }
}
