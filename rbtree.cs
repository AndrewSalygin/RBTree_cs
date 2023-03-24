using System;

class Program
{
    static void Main(string[] args)
    {
        RB tree = new RB();
        tree.Insert(13);
        tree.Insert(8);
        tree.Insert(17);
        tree.Delete(13);
        Console.ReadLine();
    }
}
enum Color
{
    Black,
    Red
}
class RB
{
    public class Node
    {
        public Color color;
        public Node left;
        public Node right;
        public Node parent;
        public int data;

        // лист
        public Node(Node parent)
        {
            color = Color.Black;
            this.parent = parent;
            data = int.MinValue;
        }

        // вершина
        public Node(int data)
        {
            left = new Node(this);
            right = new Node(this);
            this.data = data;
        }
    }
    private Node root = new Node(null);
    public RB() { }
    private void leftRotate(Node X)
    {
        Node Y = X.right; // берем правого сына узла 
        X.right = Y.left; // подхватываем левое поддерево нодой X
                          // если поддерево непустое
        if (Y.left.data != int.MinValue)
        {
            // назначаем корню поддерева родителя
            Y.left.parent = X;
        }
        // передвигаемый наверх элемент при повороте непустой
        if (Y.data != int.MinValue)
        {
            // для передвигаемого наверх элемента устанавливаем родителя предыдущего узла 
            Y.parent = X.parent;
        }
        // если родитель узла, который находился сверху пустой
        if (X.parent.data == int.MinValue)
        {
            // то корнем будет являться передвигаемый наверх элемент 
            root = Y;
        }
        // если передвигаемый вниз узел являлся левым потомком родителя
        if (X == X.parent.left)
        {
            // меняем его на Y
            X.parent.left = Y;
        }
        // иначе он был правым потомком
        else
        {
            X.parent.right = Y;
        }
        // левый потомок (новый родитель) ноды - старая нода (старый родитель)  
        Y.left = X;
        // если X не null, то его родитель передвигаемый вверх узел 
        if (X.data != int.MinValue)
        {
            X.parent = Y;
        }
    }
    private void rightRotate(Node Y)
    {
        Node X = Y.left;
        Y.left = X.right;
        if (X.right.data != int.MinValue)
        {
            X.right.parent = Y;
        }
        if (X.data != int.MinValue)
        {
            X.parent = Y.parent;
        }
        if (Y.parent.data == int.MinValue)
        {
            root = X;
        }
        if (Y == Y.parent.right)
        {
            Y.parent.right = X;
        }
        if (Y == Y.parent.left)
        {
            Y.parent.left = X;
        }
        X.right = Y;
        if (Y.data != int.MinValue)
        {
            Y.parent = X;
        }
    }

    public void DisplayTree()
    {
        if (root.data == int.MinValue)
        {
            Console.WriteLine("Nothing in the tree!");
            return;
        }
        if (root.data != int.MinValue)
        {
            InOrderDisplay(root);
        }
    }

    // поиск никак не отличается от поиска в любом бинарном дереве
    public Node Find(int key)
    {
        bool isFound = false;
        Node temp = root;
        Node item = null;
        while (!isFound)
        {
            if (temp.data == int.MinValue)
            {
                break;
            }
            if (key < temp.data)
            {
                temp = temp.left;
            }
            if (key > temp.data)
            {
                temp = temp.right;
            }
            if (key == temp.data)
            {
                isFound = true;
                item = temp;
            }
        }
        if (isFound)
        {
            Console.WriteLine("{0} was found", key);
            return temp;
        }
        else
        {
            Console.WriteLine("{0} not found", key);
            return null;
        }
    }

    public void Insert(int item)
    {
        Node newItem = new Node(item);

        // если корень пустой
        if (root.data == int.MinValue)
        {
            root = newItem;
            root.color = Color.Black;
            return;
        }

        Node Y = null;
        Node X = root;
        // пока элемент непустой спускаемся до места вставки
        while (X.data != int.MinValue)
        {
            Y = X;
            if (newItem.data < X.data)
            {
                X = X.left;
            }
            else
            {
                X = X.right;
            }
        }
        // родителем нового узла ставим узел Y
        newItem.parent = Y;
        // если узел пустой, то вставляемый элемент - это корень
        if (Y == null)
        {
            root = newItem;
        }
        // в зависимости от значения ставим влево или вправо вставляемый узел
        else if (newItem.data < Y.data)
        {
            Y.left = newItem;
        }
        else
        {
            Y.right = newItem;
        }
        // параметры любого нового листа
        newItem.left = new Node(newItem);
        newItem.right = new Node(newItem);
        newItem.color = Color.Red;

        // исправляем несбалансированность
        InsertFixUp(newItem);
    }

    // обход дерева
    private void InOrderDisplay(Node current)
    {
        if (current.data != int.MinValue)
        {
            InOrderDisplay(current.left);
            Console.Write("({0}) ", current.data);
            InOrderDisplay(current.right);
        }
    }

    private void InsertFixUp(Node item)
    {
        if (item == root)
        {
            item.color = Color.Black;
            return;
        }

        // Проверка свойств КЧД (пока отец красный нарушение свойства 3)
        while (item != root && item.parent.color == Color.Red)
        {
            // Есть нарушение (случай линия) если отец левый ребенок
            if (item.parent == item.parent.parent.left)
            {
                // значение дяди
                Node Y = item.parent.parent.right;

                // случай 1: дядя красный и он не пустой
                if (Y.data != int.MinValue && Y.color == Color.Red)
                {
                    // перекрашиваем отца
                    item.parent.color = Color.Black;
                    // перекрашиваем дядю
                    Y.color = Color.Black;
                    // перекрашивем деда
                    item.parent.parent.color = Color.Red;
                    // переходим на деда
                    item = item.parent.parent;
                }
                else // случай 2: дядя черный
                {
                    // если узел правый
                    if (item == item.parent.right)
                    {
                        // переходим на отца
                        item = item.parent;
                        // и выполняем левый поворот
                        leftRotate(item);
                    }
                    // случай 3: перекраска и поворот
                    // перекрашиваем отца в черный
                    item.parent.color = Color.Black;

                    // перекрашиваем деда в красный
                    item.parent.parent.color = Color.Red;

                    // выполняем правый поворот
                    rightRotate(item.parent.parent);
                }

            }
            else
            {
                Node Y = item.parent.parent.left;
                if (Y.data != int.MinValue && Y.color == Color.Red)
                {
                    item.parent.color = Color.Black;
                    Y.color = Color.Black;
                    item.parent.parent.color = Color.Red;
                    item = item.parent.parent;
                }
                else //Case 2
                {
                    if (item == item.parent.left)
                    {
                        item = item.parent;
                        rightRotate(item);
                    }
                    //Case 3: recolor & rotate
                    item.parent.color = Color.Black;
                    item.parent.parent.color = Color.Red;
                    leftRotate(item.parent.parent);

                }

            }
            root.color = Color.Black;
        }
    }

    private void rbTransplant(ref Node u, ref Node v)
    {
        if (u.parent == null)
        {
            root = v;
        }
        else if (u == u.parent.left)
        {
            u.parent.left = v;
        }
        else
        {
            u.parent.right = v;
        }
        v.parent = u.parent;
    }

    public void Delete(int key)
    {
        //first find the node in the tree to delete and assign to item pointer/reference
        Node z = Find(key);
        Node x;
        Node y = z;
        Color yOriginalColor = y.color;

        if (z.left.data == int.MinValue)
        {
            x = z.right;
            rbTransplant(ref z, ref z.right); 
        }
        else if (z.right.data == int.MinValue)
        {
            x = z.left;
            rbTransplant(ref z, ref z.left);
        }
        else {
            y = minimum(z.right);
            yOriginalColor = y.color;
            x = y.right;
            if (y.parent == z)
            {
                x.parent = y;
            }
            else
            {
                rbTransplant(ref y, ref y.right);
                y.right = z.right;
                y.right.parent = y;
            }
                rbTransplant(ref z, ref y);
                y.left = z.left;
                y.left.parent = y;
                y.color = z.color;
           }
            if (yOriginalColor == 0)
            {
                DeleteFixUp(x);
            }
        }
    /// <summary>
    /// Checks the tree for any violations after deletion and performs a fix
    /// </summary>
    /// <param name="X"></param>
    private void DeleteFixUp(Node X)
    {
        while (X != root && X.color == Color.Black)
        {
            // X - левый ребенок
            if (X.data != int.MinValue && X == X.parent.left)
            {
                Node B = X.parent.right;
                if (B.data != int.MinValue && B.color == Color.Red)
                {
                    B.color = Color.Black;
                    X.parent.color = Color.Red;
                    leftRotate(X.parent);
                    B = X.parent.right;
                }
                // у брата черные дети
                if (B.data != int.MinValue && B.left.color == Color.Black && B.right.color == Color.Black)
                {
                    B.color = Color.Red;
                    // X.parent.color = Color.Black;
                    X = X.parent;
                }
                else if (B.data != int.MinValue)
                {
                    if (B.right.color == Color.Black)
                    {
                        B.left.color = Color.Black; //case 3
                        B.color = Color.Red; //case 3
                        rightRotate(B); //case 3
                        B = X.parent.right; //case 3
                    }
                    B.color = X.parent.color; //case 4
                    X.parent.color = Color.Black; //case 4
                    B.right.color = Color.Black; //case 4
                    leftRotate(X.parent); //case 4
                    X = root; //case 4
                }
                else
                {
                    break;
                }
            }
            else //mirror code from above with "right" & "left" exchanged
            {
                Node B = X.parent.left;
                if (B.color == Color.Red && B.data != int.MinValue)
                {
                    B.color = Color.Black;
                    X.parent.color = Color.Red;
                    rightRotate(X.parent);
                    B = X.parent.left;
                }
                if (B.data != int.MinValue && B.right.color == Color.Black && B.left.color == Color.Black)
                {
                    // поменять на color
                    B.color = Color.Red;
                  //  X.parent.color = Color.Black;
                    X = X.parent;
                }
                else if (B.data != int.MinValue)
                {
                    if (B.left.color == Color.Black)
                    {
                        B.right.color = Color.Black;
                        B.color = Color.Red;
                        leftRotate(B);
                        B = X.parent.left;
                    }  
                        B.color = X.parent.color;
                        X.parent.color = Color.Black;
                        B.left.color = Color.Black;
                        rightRotate(X.parent);
                        X = root;
                }
                else
                {
                    break;
                }
            }
        }
        X.color = Color.Black;
    }

    private Node minimum(Node x)
    {
        while (x.left.data != int.MinValue)
        {
            x = x.left;
        }
        return x;
    }
}