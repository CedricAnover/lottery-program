
void ArrayPrinter(int[] array)
{
    for (int i = 0; i < array.Length; i++)
    {
        if (i == 0)
        {
            Console.Write($"[{array[i]}, ");
        }
        else if (i == array.Length - 1)
        {
            Console.Write($"{array[i]}]");
        }
        else
        {
            Console.Write($"{array[i]}, ");
        }
    }

    Console.Write("\n");
}

int? BinarySearch(int Elem, int[] inpArr)
{
    /* Return the Index of the Element (in the sorted array) if exist, otherwise return null */

    // Sort the given array
    int[] array = inpArr.OrderBy(x => x).ToArray(); // Sort without mutating the Original Array

    // Apply Binary Search Algorithm
    int l = 0, r = inpArr.Length - 1; // Initial Left and Right Indexes
    while (l <= r)
    {
        int mid = l + (r - l) / 2;
        if (array[mid] == Elem)
        {
            return mid;
        }

        // If Middle Element of current (l, r) index is less than Elem
        if (array[mid] < Elem)
        {
            // Update the Left Index
            l = mid + 1;
        }
        // If Middle Element of current (l, r) index is geater than or equal to Elem
        else
        {
            // Update the Right Index
            r = mid - 1;
        }
    }

    return null;
}

int? LinearSearch(int Elem, int[] inpArr)
{
    /* Return the Index of the Element if exists, otherwise return null */
    for (int i = 0; i < inpArr.Length; i++)
    {
        if (inpArr[i] == Elem)
        {
            return i;
        }
    }

    return null;
}

int[] RandomArrayGenerator(int lowestValue, int highestValue, int Size)
{
    int[] outArr = new int[Size];

    int i = 0;
    while (i < Size)
    {
        Random rnd = new Random(); // Warning: Random Class in C# is Right-Exclusive!!!
        int randNumber = rnd.Next(lowestValue, highestValue + 1);

        // Check if the number does not exist in outArr
        //int? matchIndex = BinarySearch(randNumber, outArr);
        int? matchIndex = BinarySearch(randNumber, outArr);

        if (matchIndex is null)
        {
            outArr[i] = randNumber;
            i++; // Counter update only if the generated number is new
        }
    }

    return outArr;
}

/* ###################################################################################################################### */

/* Program Parameters */
int arrSize = 3; // Size of Array as a Parameter. No need to prompt.
int minVal = 1, maxVal = 10; // Min and Max Value Range for the Lottery Numbers (Both for User and Random Number Generator)

double prizeThreshold = 0.5d;
double maxCashPrize = 1000000.00d; // Maximum Cash Prize if user guessed perfectly

int[] userArr = new int[arrSize]; // User's Lottery Numbers

/* Prompt the User for their Lottery Numbers */
int promptCounter = 0;
Console.WriteLine($"Please enter your {arrSize} lottery numbers between {minVal} and {maxVal}: \n");
while (true)
{
    Console.Write($"Enter Number {promptCounter + 1}: ");

    int tempInput; // Temporarily Store the User Input
    bool resInput = int.TryParse( Console.ReadLine(), out tempInput); // Result of the User Input

    // Check and see if user inserted valid values. If not, use continue to skip interation and prompt user again.
    if (!resInput)
    {
        Console.WriteLine("Cannot Parse the given input into integer, please try again ...\n");
        continue;
    }
    if (tempInput < minVal || tempInput > maxVal)
    {
        Console.WriteLine($"Invalid Entry! The Number must be in between {minVal} and {maxVal}. Please try again ...\n");
        continue;
    }
    if (BinarySearch(tempInput, userArr) != null)
    {
        Console.WriteLine($"You already included {tempInput}. Please try a different number ...\n");
        continue;
    }

    userArr[promptCounter] = tempInput;

    promptCounter++;

    //Console.Write("Your Current Array: ");
    //ArrayPrinter(userArr);
    //Console.Write("\n");

    // Loop Exit Criterion.
    if (promptCounter >= arrSize) { break; }
}

/* ###################################################################################################################### */
void Main()
{
    //int[] userNumbers = userArr; // Redundant to create new variable

    int[] randNumbers = RandomArrayGenerator(minVal, maxVal, arrSize); // Generate Random Lottery

    Console.Write("\nLottery Numbers: ");
    ArrayPrinter(randNumbers);

    Console.Write("\nYour Numbers: ");
    ArrayPrinter(userArr);
    Console.Write("\n");

    // Create List to Keep Track How many correct numbers the user guessed.
    // Challenged myself to use LINQ.
    int?[] matchList = new int?[arrSize];

    for (int i = 0; i < arrSize; i++)
    {
        int elem = randNumbers[i];
        matchList[i] = BinarySearch(elem, userArr) != null ? elem : null; // Carefull! Can get confused with index and elements!
    }

    // Filter out 0 & null values. Type of matchList has to be preserved & declared.
    int?[] tempMatchList = matchList.Where(x => x != null).ToArray();

    if (tempMatchList.Length == 0)
    {
        Console.WriteLine("\nSorry, you did not guessed anything correct. Better luck next time.");
    }
    else
    {
        Console.WriteLine($"\nYou managed to guess {tempMatchList.Length} number(s)! They are: ");
        foreach(int? elem in tempMatchList) // We know it will never have null because we filtered
        {
            Console.WriteLine(elem);
        }

        double percentGuessed = (double)tempMatchList.Length / (double)arrSize;
        if (percentGuessed >= prizeThreshold)
        {
            double cashPrize = percentGuessed * maxCashPrize;
            Console.WriteLine($"\nYou win ${cashPrize:n}!!!");
        }
        else
        {
            Console.WriteLine($"\nSorry No Prize. \nYou only guessed less than {prizeThreshold * 100} percent of the lottery numbers");
        }
    }
}

Main();
