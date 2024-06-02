using Repositories;

RadarRepository r = new();

var lst = r.GetDataSql();
//var dataCSV = r.GenerateCSV(lst);

//Console.WriteLine(r.GenerateXML(lst));

Console.WriteLine(r.GenerateJson(lst));

//foreach (var item in lst)
//{
//    Console.WriteLine(item);
//}