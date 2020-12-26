using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_22___Slam_Shuffle
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            ////////////////////////////////
            // Part 1
            ////////////////////////////////

            DLinkedList dLinkedList = new DLinkedList();

            const long cardsP1 = 10007;
            for (long i = 0; i < cardsP1; i++)
            {
                DLinkedListNode newNode = new DLinkedListNode(dLinkedList, i);
                dLinkedList.InsertAtTail(newNode);
            }

            foreach (string instruction in inputList)
            {
                if (instruction.Length >= 19 && instruction.Substring(0, 19) == "deal into new stack")
                {
                    dLinkedList.Reverse();
                }
                else if (instruction.Length >= 19 && instruction.Substring(0, 19) == "deal with increment")
                {
                    // deal with increment
                    long increment = long.Parse(instruction.Substring(20));

                    DLinkedList newDLinkedList = new DLinkedList();
                    for (long i = 0; i < cardsP1; i++)
                    {
                        DLinkedListNode newNode = new DLinkedListNode(newDLinkedList, null);
                        newDLinkedList.InsertAtTail(newNode);
                    }

                    DLinkedListNode currentOldNode = dLinkedList.Head;
                    DLinkedListNode currentNewNode = newDLinkedList.Head;
                    for (long i = 0; i < cardsP1; i++)
                    {
                        currentNewNode.Value = currentOldNode.Value;

                        currentNewNode = currentNewNode.Advance(increment, true);
                        currentOldNode = currentOldNode.Next;
                    }
                    dLinkedList = newDLinkedList;
                }
                else if (instruction.Substring(0, 3) == "cut")
                {
                    long cardsToCut = long.Parse(instruction.Substring(4));
                    DLinkedListNode beginningOfCutCards;
                    DLinkedListNode endOfCutCards;
                    if (cardsToCut > 0)
                    {
                        DLinkedListNode oldTail;
                        DLinkedListNode newHead;
                        beginningOfCutCards = dLinkedList.Head;
                        oldTail = dLinkedList.Tail;
                        endOfCutCards = beginningOfCutCards.Advance(cardsToCut - 1);
                        newHead = endOfCutCards.Next;

                        dLinkedList.Head = newHead;
                        newHead.Previous = null;

                        oldTail.Next = beginningOfCutCards;
                        beginningOfCutCards.Previous = oldTail;
                        dLinkedList.Tail = endOfCutCards;
                        endOfCutCards.Next = null;
                    }
                    else
                    {
                        cardsToCut = -cardsToCut;
                        DLinkedListNode newTail;
                        DLinkedListNode oldHead;
                        beginningOfCutCards = dLinkedList.Tail.AdvanceBackwards(cardsToCut - 1);
                        oldHead = dLinkedList.Head;
                        endOfCutCards = dLinkedList.Tail;
                        newTail = beginningOfCutCards.Previous;

                        dLinkedList.Tail = newTail;
                        newTail.Next = null;

                        oldHead.Previous = endOfCutCards;
                        endOfCutCards.Next = oldHead;
                        dLinkedList.Head = beginningOfCutCards;
                        beginningOfCutCards.Previous = null;
                    }
                }
            }

            long positionOf2019;
            if (dLinkedList.Find((long)2019, out positionOf2019) == null)
                throw new Exception();
            Console.WriteLine(positionOf2019);

            ////////////////////////////////
            // Part 2
            ////////////////////////////////

            dLinkedList = new DLinkedList();

            const long cardsP2 = 119315717514047;
            for (long i = 0; i < cardsP2; i++)
            {
                DLinkedListNode newNode = new DLinkedListNode(dLinkedList, i);
                dLinkedList.InsertAtTail(newNode);
            }

            Console.WriteLine("kd");

            foreach (string instruction in inputList)
            {
                if (instruction.Length >= 19 && instruction.Substring(0, 19) == "deal into new stack")
                {
                    dLinkedList.Reverse();
                }
                else if (instruction.Length >= 19 && instruction.Substring(0, 19) == "deal with increment")
                {
                    // deal with increment
                    long increment = long.Parse(instruction.Substring(20));

                    DLinkedList newDLinkedList = new DLinkedList();
                    for (long i = 0; i < cardsP2; i++)
                    {
                        DLinkedListNode newNode = new DLinkedListNode(newDLinkedList, null);
                        newDLinkedList.InsertAtTail(newNode);
                    }

                    DLinkedListNode currentOldNode = dLinkedList.Head;
                    DLinkedListNode currentNewNode = newDLinkedList.Head;
                    for (long i = 0; i < cardsP2; i++)
                    {
                        currentNewNode.Value = currentOldNode.Value;

                        currentNewNode = currentNewNode.Advance(increment, true);
                        currentOldNode = currentOldNode.Next;
                    }
                    dLinkedList = newDLinkedList;
                }
                else if (instruction.Substring(0, 3) == "cut")
                {
                    long cardsToCut = long.Parse(instruction.Substring(4));
                    DLinkedListNode beginningOfCutCards;
                    DLinkedListNode endOfCutCards;
                    if (cardsToCut > 0)
                    {
                        DLinkedListNode oldTail;
                        DLinkedListNode newHead;
                        beginningOfCutCards = dLinkedList.Head;
                        oldTail = dLinkedList.Tail;
                        endOfCutCards = beginningOfCutCards.Advance(cardsToCut - 1);
                        newHead = endOfCutCards.Next;

                        dLinkedList.Head = newHead;
                        newHead.Previous = null;

                        oldTail.Next = beginningOfCutCards;
                        beginningOfCutCards.Previous = oldTail;
                        dLinkedList.Tail = endOfCutCards;
                        endOfCutCards.Next = null;
                    }
                    else
                    {
                        cardsToCut = -cardsToCut;
                        DLinkedListNode newTail;
                        DLinkedListNode oldHead;
                        beginningOfCutCards = dLinkedList.Tail.AdvanceBackwards(cardsToCut - 1);
                        oldHead = dLinkedList.Head;
                        endOfCutCards = dLinkedList.Tail;
                        newTail = beginningOfCutCards.Previous;

                        dLinkedList.Tail = newTail;
                        newTail.Next = null;

                        oldHead.Previous = endOfCutCards;
                        endOfCutCards.Next = oldHead;
                        dLinkedList.Head = beginningOfCutCards;
                        beginningOfCutCards.Previous = null;
                    }
                }
            }

            Console.WriteLine();

            Console.ReadLine();
        }
    }

    public class DLinkedList
    {
        public long Count = 0;
        public DLinkedListNode Head = null;
        public DLinkedListNode Tail = null;

        public void Reverse()
        {
            DLinkedListNode currentNode = this.Head;
            while (currentNode != this.Tail)
            {
                currentNode.FlipOrder();
                currentNode = currentNode.Previous;
            }
            currentNode.FlipOrder();

            DLinkedListNode temp = this.Head;
            this.Head = this.Tail;
            this.Tail = temp;
        }

        public void InsertAtHead(DLinkedListNode newNode)
        {
            Count++;
            if (this.Head == null)
            {
                this.Head = newNode;
                this.Tail = newNode;
            }
            else
            {
                newNode.Next = this.Head;
                this.Head.Previous = newNode;
                this.Head = newNode;
            }
        }

        public void InsertAtTail(DLinkedListNode newNode)
        {
            Count++;
            if (this.Tail == null)
            {
                this.Tail = newNode;
                this.Head = newNode;
            }
            else
            {
                newNode.Previous = this.Tail;
                this.Tail.Next = newNode;
                this.Tail = newNode;
            }
        }

        public DLinkedListNode Find(object value, out long pos)
        {
            pos = 0;
            DLinkedListNode currentNode = this.Head;
            while (currentNode != this.Tail)
            {
                if (currentNode.Value.Equals(value)) return currentNode;
                pos++;
                currentNode = currentNode.Next;
            }
            if (currentNode.Value.Equals(value)) return currentNode;
            return null;
        }

        public override string ToString()
        {
            string val = "";
            DLinkedListNode currentNode = this.Head;
            while (currentNode != this.Tail)
            {
                val += currentNode.ToString() + ",";
                currentNode = currentNode.Next;
            }
            val += currentNode.ToString() + ",";
            if (val.Length > 0)
                val = val.Substring(0, val.Length - 1);
            return val;
        }
    }

    public class DLinkedListNode
    {
        public DLinkedList List;
        public DLinkedListNode Previous;
        public DLinkedListNode Next;
        public object Value;

        public DLinkedListNode(DLinkedList dLinkedList, object value)
        {
            this.List = dLinkedList;
            this.Value = value;
        }

        public void FlipOrder()
        {
            var temp = this.Next;
            this.Next = this.Previous;
            this.Previous = temp;
        }

        public DLinkedListNode Advance(long n, bool wrap = false)
        {
            DLinkedListNode ret = this;
            for (long i = 0; i < n; i++)
                if (ret.Next != null || !wrap)
                    ret = ret.Next;
                else
                    ret = this.List.Head;
            return ret;
        }

        public DLinkedListNode AdvanceBackwards(long n, bool wrap = false)
        {
            DLinkedListNode ret = this;
            for (long i = 0; i < n; i++)
                if (ret.Previous != null || !wrap)
                    ret = ret.Previous;
                else
                    ret = this.List.Tail;
            return ret;
        }

        public override string ToString()
        {
            if (Value != null)
                return Value.ToString();
            else
                return "null";
        }
    }
}
