using Microsoft.AspNetCore.Http;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public static class HttpContextExtensions
{
    public static async Task Container(this StringBuilder sb)
    {

    }
    public static async Task Navbar(this StringBuilder sb)
    {

    }
    public static async Task List( this StringBuilder sb )
    {

    }
    public static async Task Bootstrap(this HttpContext http, Action<StringBuilder> Bootstrap)
    {
        http.Response.ContentType = "text/html";
        var sb = new StringBuilder();
        sb.Append(@"
        <!doctype html>
        <html lang=""en"">
          <head>
            <meta charset=""utf-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
            <title>Bootstrap demo</title>
            <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-gH2yIJqKdNHPEq0n4Mqa/HGKIhSkIHeL5AyhkYV8i59U5AR6csBvApHHNl/vI1Bx"" crossorigin=""anonymous"">
          </head>
          <body>
           ");

        Bootstrap(sb);
        sb.Append(@"
            <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/js/bootstrap.bundle.min.js"" integrity=""sha384-A3rJD856KowSb7dwlZdYEkO39Gagi7vIsF0jrRAoQmDKKtQBHUuLZ9AsSv4jD4Xa"" crossorigin=""anonymous""></script>
          </body>
        </html>
        ");
        await Task.CompletedTask;
    }
    public static async Task Table(this HttpContext http, string title, IEnumerable<string> columns, JArray items)
    {
        http.Response.ContentType = "text/html";
        var sb = new StringBuilder();
        sb.Append(@"
        <!doctype html>
        <html lang=""en"">
          <head>
            <meta charset=""utf-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
            <title>Bootstrap demo</title>
            <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-gH2yIJqKdNHPEq0n4Mqa/HGKIhSkIHeL5AyhkYV8i59U5AR6csBvApHHNl/vI1Bx"" crossorigin=""anonymous"">
          </head>
          <body>
           ");

        sb.Append(@$"<h2>{title}</h2>");
        sb.Append(@$"<table class=""table table-bordered"">");
        sb.Append(@$"<tr>");
        foreach (string column in columns)
            sb.Append(@$"<td>column{column}</td>");

        sb.Append(@$"</tr>");
/*
        foreach (   JToken item in items)
        {
            sb.Append(@$"<tr>");

            foreach (string column in columns)

                sb.Append(@$"<td>{item[column] }</td>");
            sb.Append(@$"</tr>");



        }*/
        sb.Append(@$"</table>");

        
        sb.Append(@"
            <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/js/bootstrap.bundle.min.js"" integrity=""sha384-A3rJD856KowSb7dwlZdYEkO39Gagi7vIsF0jrRAoQmDKKtQBHUuLZ9AsSv4jD4Xa"" crossorigin=""anonymous""></script>
          </body>
        </html>
        ");
        var html = sb.ToString();
        Console.WriteLine(html);
        await http.Response.WriteAsync(html);
        await Task.CompletedTask;
    }
    public static async Task List(this HttpContext http, string title, IEnumerable<KeyValuePair<string, string>> keyValues)
    {
        http.Response.ContentType = "text/html";
        var sb = new StringBuilder();
        sb.Append(@"
        <!doctype html>
        <html lang=""en"">
          <head>
            <meta charset=""utf-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
            <title>Bootstrap demo</title>
            <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-gH2yIJqKdNHPEq0n4Mqa/HGKIhSkIHeL5AyhkYV8i59U5AR6csBvApHHNl/vI1Bx"" crossorigin=""anonymous"">
          </head>
          <body>
           ");

        sb.Append(@$"<h2>{title}</h2>");
        sb.Append(@"<ul class=""list-group"">");
        foreach (var kv in keyValues)
            sb.Append($@"<li class=""list-group-item""><a href=""{kv.Value}"">{kv.Key}<a></li>");
        sb.Append(@"</ul>");
        await http.Response.WriteAsync(sb.ToString());
        sb.Append(@"
            <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/js/bootstrap.bundle.min.js"" integrity=""sha384-A3rJD856KowSb7dwlZdYEkO39Gagi7vIsF0jrRAoQmDKKtQBHUuLZ9AsSv4jD4Xa"" crossorigin=""anonymous""></script>
          </body>
        </html>
        ");
        await Task.CompletedTask;
    }
    public static async Task List(this HttpContext http, string title, IEnumerable<string> options)
    {
        http.Response.ContentType = "text/html";
        var sb = new StringBuilder();
        sb.Append(@"
        <!doctype html>
        <html lang=""en"">
          <head>
            <meta charset=""utf-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
            <title>Bootstrap demo</title>
            <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-gH2yIJqKdNHPEq0n4Mqa/HGKIhSkIHeL5AyhkYV8i59U5AR6csBvApHHNl/vI1Bx"" crossorigin=""anonymous"">
          </head>
          <body>
           ");

        sb.Append(@$"<h2>{title}</h2>");
        sb.Append(@"<ul class=""list-group"">");
        foreach (var kv in options)
            sb.Append($@"<li class=""list-group-item""><a href=""{kv}"">{kv}<a></li>");
        sb.Append(@"</ul>");
        await http.Response.WriteAsync(sb.ToString());
        sb.Append(@"
            <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/js/bootstrap.bundle.min.js"" integrity=""sha384-A3rJD856KowSb7dwlZdYEkO39Gagi7vIsF0jrRAoQmDKKtQBHUuLZ9AsSv4jD4Xa"" crossorigin=""anonymous""></script>
          </body>
        </html>
        ");
        await Task.CompletedTask;
    }
}

