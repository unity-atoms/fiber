namespace FiberUtils
{
    public struct IntIdGenerator
    {
        private int _nextId;
        public const int DEFAULT_INITIAL_NEXT_ID = 1;
        public const int EMPTY_ID = 0;

        public IntIdGenerator(int nextId = DEFAULT_INITIAL_NEXT_ID)
        {
            _nextId = nextId;
        }

        public void Initialize(int nextId = DEFAULT_INITIAL_NEXT_ID)
        {
            _nextId = nextId;
        }

        public int CurrentId()
        {
            return _nextId;
        }

        public int NextId()
        {
            return _nextId++;
        }
    }
}