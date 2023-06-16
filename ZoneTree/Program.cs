using ZoneTree.Configurations;
using ZoneTreeSample.Caching;

//var znTest = new ZoneTreeIntTesting();
//var elaspedIntInsertMs = znTest.InsertIntData();
//var elaspedIntInsertParallelMs = znTest.InsertIntData(true);
//var elapedIntIterateTime = znTest.IterateIntData();

//var znStrTest = new ZoneTreeStringTesting();
//var elaspedStringInsertMs = znStrTest.InsertStringData();
//var elaspedStringInsertParallelMs = znStrTest.InsertStringData(true);
//var elapedStringIterateTime = znStrTest.IterateStringData();


Console.WriteLine($"Benchamarks for {ZoneTreeConfig.ItemCount} items.");

var znCmplxTest = new ZoneTreeComplexTesting();

var msComplexInsert = znCmplxTest.InserComplexData();
var msComplexParallelInsert = znCmplxTest.InserComplexData(true);
var msComplexIterateTime = znCmplxTest.IterateComplexData();

//Console.WriteLine($"{msComplexInsert / 1000}s");
//Console.WriteLine($"{msComplexParallelInsert / 1000}s");
//Console.WriteLine($"{msComplexIterateTime / 1000}s");

//Console.WriteLine($"elapsed miliiseconds for normal int insert: {elaspedIntInsertMs}", ConsoleColor.Green);
//Console.WriteLine($"elapsed miliiseconds for parallel int insert: {elaspedIntInsertParallelMs}", ConsoleColor.Green);
//Console.WriteLine($"elapsed miliiseconds to iterate int: {elapedIntIterateTime}", ConsoleColor.Green);

Console.WriteLine($"elapsed seconds for normal insert: {msComplexInsert / 1000}s", ConsoleColor.Green);
Console.WriteLine($"elapsed seconds for parallel insert: {msComplexParallelInsert / 1000}s", ConsoleColor.Green);
Console.WriteLine($"elapsed seconds to iterate: {msComplexIterateTime / 1000}s", ConsoleColor.Green);