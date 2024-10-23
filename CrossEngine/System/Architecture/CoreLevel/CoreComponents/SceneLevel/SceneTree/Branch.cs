namespace CrossEngine.System
{
    internal class Branch
    {
        public int Level { get; }
        public Branch[] Branches { get; }
        public bool IsOpen { get => _isOpen; set { _isOpen = IsParent && value; } }
        private bool _isOpen;
        public bool IsParent => GameObject.Transform.ChildCount > 0;
        public GameObject GameObject { get; }

        public Branch(GameObject rootNode, int level = 0)
        {
            Level = level;
            GameObject = rootNode;
            IsOpen = true;
            Branches = new Branch[rootNode.Transform.ChildCount];

            Transform[] childs = rootNode.Transform.GetChilds();
            for (int i = 0; i < childs.Length; i++)
            {
                Branches[i] = new Branch(childs[i].GameObject, Level + 1);
            }
        }
    }
}
