namespace RestServiceB.Helper
{
    public class ReadWriteFromTextFile
    {
        public void ReadAndUpdateLastSumValue(int lastSumFromQueueMessage)
        {
            // Implementation for reading and writing to a static text file
            int lastSumValue = int.Parse(File.ReadAllText("lastSum.txt"));

            lastSumValue += lastSumFromQueueMessage;

            File.WriteAllText("lastSum.txt", lastSumValue.ToString());
        }
    }
}
