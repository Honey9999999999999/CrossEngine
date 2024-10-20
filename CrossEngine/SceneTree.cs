using CrossEngine.System;

namespace CrossEngine
{
    internal class SceneTree : CrossBehaviour
    {
        private int level;

        public override void Update()
        {
            base.Update();

            Console.WriteLine($"\nScene : {SceneManager.GetActiveScene().Name}");

            WriteChilds(SceneManager.GetActiveScene().RootNode.Transform.GetChilds(), "RootObject");

            level = 0;
        }

        private void WriteChilds(Transform[] childs, string name = "Child")
        {
            level++;

            int cursorTop = 0;
            int cursorTopLast = Console.CursorTop + 1;

            for (int i = 0; i < childs.Length; i++)
            {

                int upperLevel = level * 2 - 1;
                Console.CursorLeft = upperLevel;

                if (i != 0)
                {
                    for (int j = 0; j < cursorTop - cursorTopLast; j++)
                    {
                        Console.SetCursorPosition(upperLevel, cursorTop - j - 1);
                        Console.Write("│");
                    }

                    Console.SetCursorPosition(upperLevel, cursorTop);
                }

                cursorTopLast = Console.CursorTop + 1;

                Console.WriteLine($"{(i == childs.Length - 1 ? "└" : "├")}{name} : {childs[i].GameObject.Name}");

                level++;

                Component[] components = childs[i].GetComponents();
                for (int j = 0; j < components.Length; j++)
                {
                    Console.CursorLeft = level * 2;
                    Console.WriteLine($"{(j == components.Length - 1 ? "└" : "├")}Object component : {components[j].GetType().Name}");
                }

                level--;

                Transform[] newChilds = childs[i].GetChilds();

                if (newChilds.Length > 0)
                {
                    int interCursorTop = Console.CursorTop - 1;
                    int interCursorLeft = level * 2 + 1;

                    WriteChilds(newChilds);

                    int currentTop = Console.CursorTop;

                    for (int j = 0; j < childs[i].GetComponents().Length; j++)
                    {
                        Console.SetCursorPosition(interCursorLeft, interCursorTop - j);
                        Console.Write("│");
                    }

                    Console.CursorTop = currentTop;
                }

                cursorTop = Console.CursorTop;
            }

            level--;
        }
    }
}
