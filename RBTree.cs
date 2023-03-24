using System;

namespace RBTree {
    enum Color {
        Black,
        Red
    }
    class RBTree {
        public class Node {
            public Color color;
            public Node left;
            public Node right;
            public Node parent;
            public int data;

            // лист
            public Node(Node parent) {
                color = Color.Black;
                this.parent = parent;
                data = int.MinValue;
            }

            // вершина
            public Node(int data) {
                left = new Node(this);
                right = new Node(this);
                this.data = data;
            }
        }
        private Node root = new Node(null);
        public RBTree() { }
        private void leftRotate(Node x) {
            Node y = x.right; // берем правого сына узла 
            x.right = y.left; // подхватываем левое поддерево нодой X
                              // если поддерево непустое
            if (y.left.data != int.MinValue) {
                // назначаем корню поддерева родителя
                y.left.parent = x;
            }
            // передвигаемый наверх элемент при повороте непустой
            if (y.data != int.MinValue) {
                // для передвигаемого наверх элемента устанавливаем родителя предыдущего узла 
                y.parent = x.parent;
            }
            // если родитель узла, который находился сверху пустой
            if (x.parent.data == int.MinValue) {
                // то корнем будет являться передвигаемый наверх элемент 
                root = y;
            }
            // если передвигаемый вниз узел являлся левым потомком родителя
            if (x == x.parent.left) {
                // меняем его на Y
                x.parent.left = y;
            }
            // иначе он был правым потомком
            else {
                x.parent.right = y;
            }
            // левый потомок (новый родитель) ноды - старая нода (старый родитель)  
            y.left = x;
            // если X не null, то его родитель передвигаемый вверх узел 
            if (x.data != int.MinValue) {
                x.parent = y;
            }
        }
        private void rightRotate(Node y) {
            Node x = y.left;
            y.left = x.right;
            if (x.right.data != int.MinValue) {
                x.right.parent = y;
            }
            if (x.data != int.MinValue) {
                x.parent = y.parent;
            }
            if (y.parent.data == int.MinValue) {
                root = x;
            }
            if (y == y.parent.right) {
                y.parent.right = x;
            }
            if (y == y.parent.left) {
                y.parent.left = x;
            }
            x.right = y;
            if (y.data != int.MinValue) {
                y.parent = x;
            }
        }

        public void DisplayTree() {
            if (root.data == int.MinValue) {
                throw new EmptyTreeException("Дерево пустое.");
            }
            if (root.data != int.MinValue) {
                InOrderDisplay(root);
            }
        }

        // поиск никак не отличается от поиска в любом бинарном дереве
        public Node Find(int key) {
            bool isFound = false;
            Node temp = root;
            Node item = null;
            while (!isFound) {
                if (temp.data == int.MinValue) {
                    break;
                }
                if (key < temp.data) {
                    temp = temp.left;
                }
                if (key > temp.data) {
                    temp = temp.right;
                }
                if (key == temp.data) {
                    isFound = true;
                    item = temp;
                }
            }
            if (isFound) {
                Console.WriteLine("{0} was found", key);
                return temp;
            } else {
                Console.WriteLine("{0} not found", key);
                return null;
            }
        }

        public void Insert(int item) {
            Node newItem = new Node(item);

            // если корень пустой
            if (root.data == int.MinValue) {
                root = newItem;
                root.color = Color.Black;
                return;
            }

            Node y = null;
            Node x = root;
            // пока элемент непустой спускаемся до места вставки
            while (x.data != int.MinValue) {
                y = x;
                if (newItem.data < x.data) {
                    x = x.left;
                } else {
                    x = x.right;
                }
            }
            // родителем нового узла ставим узел Y
            newItem.parent = y;
            // если узел пустой, то вставляемый элемент - это корень
            if (y == null) {
                root = newItem;
            }
            // в зависимости от значения ставим влево или вправо вставляемый узел
            else if (newItem.data < y.data) {
                y.left = newItem;
            } else {
                y.right = newItem;
            }
            // параметры любого нового листа
            newItem.left = new Node(newItem);
            newItem.right = new Node(newItem);
            newItem.color = Color.Red;

            // исправляем несбалансированность
            InsertFixUp(newItem);
        }

        // обход дерева
        private void InOrderDisplay(Node current) {
            if (current.data != int.MinValue) {
                InOrderDisplay(current.left);
                Console.Write("({0}) ", current.data);
                InOrderDisplay(current.right);
            }
        }

        private void InsertFixUp(Node item) {
            if (item == root) {
                item.color = Color.Black;
                return;
            }

            // Проверка свойств КЧД (пока отец красный нарушение свойства 3)
            while (item != root && item.parent.color == Color.Red) {
                // Есть нарушение (случай линия) если отец левый ребенок
                if (item.parent == item.parent.parent.left) {
                    // значение дяди
                    Node y = item.parent.parent.right;

                    // случай 1: дядя красный и он не пустой
                    if (y.data != int.MinValue && y.color == Color.Red) {
                        // перекрашиваем отца
                        item.parent.color = Color.Black;
                        // перекрашиваем дядю
                        y.color = Color.Black;
                        // перекрашивем деда
                        item.parent.parent.color = Color.Red;
                        // переходим на деда
                        item = item.parent.parent;
                    } else // случай 2: дядя черный
                      {
                        // если узел правый
                        if (item == item.parent.right) {
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

                } else {
                    Node y = item.parent.parent.left;
                    if (y.data != int.MinValue && y.color == Color.Red) {
                        item.parent.color = Color.Black;
                        y.color = Color.Black;
                        item.parent.parent.color = Color.Red;
                        item = item.parent.parent;
                    } else {
                        if (item == item.parent.left) {
                            item = item.parent;
                            rightRotate(item);
                        }
                        item.parent.color = Color.Black;
                        item.parent.parent.color = Color.Red;
                        leftRotate(item.parent.parent);

                    }

                }
                root.color = Color.Black;
            }
        }

        private void rebinding(ref Node y, ref Node x) {
            if (y.parent == null) {
                root = x;
            } else if (y == y.parent.left) {
                y.parent.left = x;
            } else {
                y.parent.right = x;
            }
            x.parent = y.parent;
        }

        public void Delete(int key) {
            Node z = Find(key);
            Node x;
            Node y = z;
            Color yOriginalColor = y.color;

            // удаляем старый элемент (делаем перепривязку)
            if (z.left.data == int.MinValue) {
                x = z.right;
                rebinding(ref z, ref z.right);
            } else if (z.right.data == int.MinValue) {
                x = z.left;
                rebinding(ref z, ref z.left);
            } else {
                // ищем минимальный элемент в правом поддереве
                y = minimum(z.right);
                yOriginalColor = y.color;
                // сохраняем правое поддерево минимального элемента
                x = y.right;
                // Случай не линии (три в ряд)
                if (y.parent != z) {
                    // делаем перепривязку (меняем удаляемую вершину z на минимальную вершину y)
                    rebinding(ref y, ref y.right);
                    y.right = z.right;
                    y.right.parent = y;
                }
                // делаем привязку отца удаляемой вершины к новой вершине y
                rebinding(ref z, ref y);
                // делаем привязку к левой вершине старой ноды к новой вершине y
                y.left = z.left;
                y.left.parent = y;
                // из-за этого нуженy yOriginalColor    
                y.color = z.color;
            }
            // запускаем проверку на баланс от сына удаляемой ноды, если нода была черная
            if (yOriginalColor == 0) {
                DeleteFixUp(x);
            }
        }

        private void DeleteFixUp(Node x) {
            while (x != root && x.color == Color.Black) {
                // x - левый ребенок
                if (x.data != int.MinValue && x == x.parent.left) {
                    // брат x
                    Node b = x.parent.right;
                    // если брат красный
                    if (b.data != int.MinValue && b.color == Color.Red) {
                        // брата красим в черный
                        b.color = Color.Black;
                        // отца в красный
                        x.parent.color = Color.Red;
                        leftRotate(x.parent);
                        // переходим к случаю, когда брат черный
                        // и теперь будем выравнивать высоту
                        b = x.parent.right;
                    }
                    // у брата черные дети
                    if (b.data != int.MinValue && b.left.color == Color.Black && b.right.color == Color.Black) {
                        // красим брата в красный
                        b.color = Color.Red;
                        // переходим к отцу и балансируем от него
                        x = x.parent;
                    }
                    // если у брата есть красный ребенок
                    else if (b.data != int.MinValue) {
                        // если левый ребенок брата (племянник) красный
                        if (b.right.color == Color.Black) {
                            b.left.color = Color.Black;
                            b.color = Color.Red;
                            rightRotate(b);
                            // переходим к случаю, когда брат черный,
                            // а племянник красный
                            b = x.parent.right;
                        }
                        // если правый племянник красный
                        // цвет отца нам не важен, поэтому это один случай
                        b.color = x.parent.color;
                        // красим отца в черный
                        x.parent.color = Color.Black;
                        // красим племянника в черный
                        b.right.color = Color.Black;
                        // делаем поворот влево от отца
                        leftRotate(x.parent);
                        x = root;
                    } else {
                        break;
                    }
                } else // аналогично предыдущему случаю, меняя "right" и "left"
                  {
                    Node b = x.parent.left;
                    if (b.color == Color.Red && b.data != int.MinValue) {
                        b.color = Color.Black;
                        x.parent.color = Color.Red;
                        rightRotate(x.parent);
                        b = x.parent.left;
                    }
                    if (b.data != int.MinValue && b.right.color == Color.Black && b.left.color == Color.Black) {
                        b.color = Color.Red;
                        x = x.parent;
                    } else if (b.data != int.MinValue) {
                        if (b.left.color == Color.Black) {
                            b.right.color = Color.Black;
                            b.color = Color.Red;
                            leftRotate(b);
                            b = x.parent.left;
                        }
                        b.color = x.parent.color;
                        x.parent.color = Color.Black;
                        b.left.color = Color.Black;
                        rightRotate(x.parent);
                        x = root;
                    } else {
                        break;
                    }
                }
            }
            x.color = Color.Black;
        }

        private Node minimum(Node x) {
            while (x.left.data != int.MinValue) {
                x = x.left;
            }
            return x;
        }
    }
}
