namespace RestServiceB.Helper
{
    public class ReadWriteFromTextFile
    {
        private readonly string _filePath;

        public ReadWriteFromTextFile()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lastSum.txt");
        }

        public int ReadAndUpdateLastSumValue(int lastSumFromQueueMessage)
        {
            int lastSumValue = 0;

            if (File.Exists(_filePath))
            {
                lastSumValue = int.Parse(File.ReadAllText(_filePath));
            }

            lastSumValue += lastSumFromQueueMessage;

            File.WriteAllText(_filePath, lastSumValue.ToString());

            return lastSumValue;
        }
    }

}
