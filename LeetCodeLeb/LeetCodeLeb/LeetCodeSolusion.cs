using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCodeLeb
{
    public class LeetCodeSolusion
    {
        public bool IsValid(string s)
        {
            Stack<char> tempStack = new Stack<char>();
            foreach (var item in s)
            {
                switch (item)
                {
                    case ')':
                        if (tempStack.Count == 0 || tempStack.Pop() != '(')
                            return false;
                        break;
                    case ']':
                        if (tempStack.Count == 0 || tempStack.Pop() != '[')
                            return false;
                        break;
                    case '}':
                        if (tempStack.Count == 0 || tempStack.Pop() != '{')
                            return false;
                        break;
                    default:
                        if (item == '(' || item == '[' || item == '{')
                        {
                            tempStack.Push(item);
                        }
                        break;
                }
            }
            if (tempStack.Count > 0)
                return false;
            return true;
        }

        public IList<IList<int>> ThreeSum(int[] nums)
        {
            var numsList = nums.ToList();
            numsList.Sort();
            IList<IList<int>> result = new List<IList<int>>();
            for (int i = 0; i < nums.Length - 2; i++)
            {
                for (int n = i + 1; n < nums.Length - 1; n++)
                {
                    for (int m = n + 1; m < nums.Length; m++)
                    {
                        if (numsList[i] + numsList[n] + numsList[m] == 0)
                        {
                            result.Add(new List<int>() { numsList[i], numsList[n], numsList[m] });
                        }
                    }
                }
            }
            return result;
        }

        public bool IsMatch(string s, string p)
        {
            var list = FindAllMatch(p);
            bool temp = false;
            var result = FindAllStatic(s, list, 0, ref temp);
            return result;
        }
        private List<Match> FindAllMatch(string s)
        {
            List<Match> list = new List<Match>();
            StringBuilder sb = new StringBuilder();
            int i = 0;
            Match match = new Match() { type = 2 };
            while (i < s.Length)
            {
                if (i + 1 < s.Length && s[i + 1] == '*')
                {
                    match = new Match();
                    match.val = new string(s[i], 1);
                    match.type = 2;
                    list.Add(match);
                    i += 2;
                }
                else if (s[i] == '.')
                {
                    match = new Match();
                    match.type = 1;
                    list.Add(match);
                    i++;
                }
                else
                {
                    if (match.type == 0)
                        match.val += s[i];
                    else
                    {
                        match = new Match() { val = new string(s[i], 1), type = 0 };
                        list.Add(match);
                    }
                    i++;
                }
            }
            return list;
        }
        private bool FindAllStatic(string s, List<Match> matchs, int Index, ref bool NeedBack)
        {
            bool IsEnd = Index == matchs.Count - 1;
            if (s.Length >= 1)
            {
                if (Index == matchs.Count)
                    return false;
                var match = matchs[Index];
                switch (match.type)
                {
                    case 0:
                        if (IsEnd)
                        {
                            if (s == match.val)
                                return true;
                            else if (s.Length < match.val.Length)
                                NeedBack = true;
                        }
                        else if (s.StartsWith(match.val))
                        {
                            return FindAllStatic(s.Remove(0, match.val.Length), matchs, Index + 1, ref NeedBack);
                        }
                        return false;
                    case 1:
                        if (IsEnd)
                        {
                            return s.Length == 1;
                        }
                        else
                        {
                            return FindAllStatic(s.Remove(0, 1), matchs, Index + 1, ref NeedBack);
                        }
                    case 2:
                        bool needBack = false;
                        if (FindAllStatic(s, matchs, Index + 1, ref needBack))
                        {
                            return true;
                        }
                        else
                        {
                            if (needBack)
                            {
                                NeedBack = true;
                                return false;
                            }
                        }
                        for (int i = 0; i < s.Length; i++)
                        {
                            if (match.val == ".")
                            {
                                if (FindAllStatic(s.Substring(i + 1), matchs, Index + 1, ref needBack))
                                    return true;
                                else if (needBack)
                                {
                                    NeedBack = true;
                                    return false;
                                }
                            }
                            else if (s[i] == match.val[0])
                            {
                                if (FindAllStatic(s.Substring(i + 1), matchs, Index + 1, ref needBack))
                                    return true;
                                else
                                    if (needBack)
                                {
                                    NeedBack = true;
                                    return false;
                                }
                            }
                            else { return false; }
                        }
                        break;
                    default: break;
                }
            }
            else
            {
                if (Index == matchs.Count)
                    return true;
                if (matchs[Index].type == 2)
                {
                    if (IsEnd)
                        return true;
                    else
                        return FindAllStatic(s, matchs, Index + 1, ref NeedBack);
                }
            }
            NeedBack = true;
            return false;
        }
        private class Match
        {
            public string val;
            public int type;//0 nomal,1 .,2 *
        }

        public int Reverse(int x)
        {
            var s = x.ToString().ToList();
            StringBuilder sb = new StringBuilder();
            int result = 0;
            int startIndex = 0;
            if (x < 0)
            {
                startIndex = 1;
                sb.Append('-');
            }
            for (int i = s.Count - 1; i >= startIndex; i--)
            {
                sb.Append(s[i]);
            }
            int.TryParse(sb.ToString(), out result);
            return result;
        }

        public double FindMedianSortedArrays(int[] nums1, int[] nums2)
        {
            int a1, a2, b1, b2, t1, t2, t3;
            a1 = a2 = b1 = b2 = 0;
            b1 = nums1.Length - 1;
            b2 = nums2.Length - 1;
            t1 = 1; t2 = 1;
            t3 = nums1.Length + nums2.Length;
            bool a1NeedAdd = false;
            bool a2NeedAdd = false;
            bool b1NeedDown = false;
            bool b2NeedDown = false;
            int temp1 = 0;
            int temp2 = 0;
            while (t1 + t2 <= t3 + 1)
            {
                a1NeedAdd = a2NeedAdd = b1NeedDown = b2NeedDown = false;
                if ((a2 <= b2) && (a1 > b1 || nums1[a1] >= nums2[a2]))
                {
                    temp1 = nums2[a2];
                    a2NeedAdd = true;
                    t1++;
                }
                else if (a1 <= b1)
                {
                    temp1 = nums1[a1];
                    a1NeedAdd = true;
                    t1++;
                }

                if ((b1 >= a1) && (b2 < a2 || nums1[b1] >= nums2[b2]))
                {
                    temp2 = nums1[b1];
                    b1NeedDown = true;
                    t2++;
                }
                else if (b2 >= a2)
                {
                    temp2 = nums2[b2];
                    b2NeedDown = true;
                    t2++;
                }
                if (a2NeedAdd) a2++;
                if (a1NeedAdd) a1++;
                if (b1NeedDown) b1--;
                if (b2NeedDown) b2--;

            }
            return (temp1 + temp2) / 2.0;
        }

        public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            int a = 0;
            int b = 0;
            bool needUpOne = false;
            ListNode result = new ListNode(0);
            ListNode TempNode = result;
            while (l1 != null || l2 != null || needUpOne)
            {
                a = b = 0;
                if (l1 != null)
                {
                    a = l1.val;
                    l1 = l1.next;
                }
                if (l2 != null)
                {
                    b = l2.val;
                    l2 = l2.next;
                }
                TempNode.next = new ListNode(a + b + (needUpOne ? 1 : 0));
                TempNode = TempNode.next;
                needUpOne = TempNode.val >= 10;
                TempNode.val -= (needUpOne ? 10 : 0);
            }
            return result.next;
        }

        public int[] TwoSum(int[] nums, int target)
        {
            var list = sum(nums, target);
            int[] result = new int[2];
            int count = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] == list[0] || nums[i] == list[1])
                {
                    result[count++] = i + 1;
                    if (count == 2)
                        return result;
                }
            }
            return result;
        }

        public int[] sum(int[] nums, int target)
        {
            var list = nums.ToList();
            list.Sort();
            int a = 0;
            int b = list.Count - 1;
            while (a != b)
            {
                var value = list[a] + list[b];
                if (value == target)
                {
                    return new int[2] { list[a], list[b] };
                }
                else if (value > target)
                    b--;
                else a++;
            }
            return null;
        }

        public ListNode RemoveElements(ListNode head, int val)
        {
            ListNode result = head;
            if (head != null)
            {
                if (head.next != null)
                {
                    do
                    {
                        while (head.next != null && head.next.val == val)
                        {
                            head.next = head.next.next;
                        }
                        if (head.next != null)
                            head = head.next;
                    }
                    while (head != null && head.next != null);
                }
                if (result.val == val)
                {
                    result = result.next;
                }
            }
            return result;
        }
        public IList<string> SummaryRanges(int[] nums)
        {
            IList<string> result = new List<string>();
            if (nums.Length > 0)
            {
                int s = nums[0];
                int temp = nums[0];
                if (nums.Length == 1)
                    result.Add(s.ToString());
                else
                {
                    for (int x = 1; x < nums.Length; x++)
                    {
                        if (nums[x] - 1 > temp)
                        {
                            if (temp == s)
                                result.Add(s.ToString());
                            else
                                result.Add(s + "->" + temp);
                            s = nums[x];
                        }
                        temp = nums[x];
                    }
                    if (temp == s)
                        result.Add(s.ToString());
                    else
                        result.Add(s + "->" + temp);
                }
            }
            return result;
        }

        public TreeNode InvertTree(TreeNode root)
        {
            if (root != null)
            {
                InvertTree(root.left);
                InvertTree(root.right);
                TreeNode temp = root.left;
                root.left = root.right;
                root.right = temp;
            }
            return root;
        }

        public int ComputeArea(int A, int B, int C, int D, int E, int F, int G, int H)
        {
            int x = ShortLine(A, C, E, G);
            int y = ShortLine(B, D, F, H);
            return Squer(A, B, C, D) + Squer(E, F, G, H) - x * y;
            //return (C - A) * (D - B) + (G - E) * (H - F) - x * y;
        }

        public int Squer(int a, int b, int c, int d)
        {
            return (c - a) * (d - b);
        }

        public int ShortLine(int a, int b, int c, int d)
        {
            if (a > c)
            {
                if (a > d)
                    return 0;   // c-d a-b
                if (b > d)
                    return d - a; //c-a=d-b
                return b - a; //c-a=b-d
            }
            else
            {
                if (c > b) //a-b c-d
                    return 0;
                if (d > b) //a-c=b-d
                    return b - c;
                return d - c; //a-c=d-b
            }
        }
        public int CountNodes(TreeNode root)
        {
            if (root == null)
                return 0;
            int leftHeight = findLeftLeafHeight(root.left);
            int rightHeight = findRightLeafHeight(root.right);
            if (leftHeight == rightHeight)
            {
                return (int)Math.Pow(2, leftHeight + 1) - 1;
            }
            else
                return findChangeNode(root, leftHeight, rightHeight);
        }

        private int findLeftLeafHeight(TreeNode root)
        {
            int count = 0;
            while (root != null)
            {
                count++;
                root = root.left;
            }
            return count;
        }
        private int findRightLeafHeight(TreeNode root)
        {
            int count = 0;
            while (root != null)
            {
                count++;
                root = root.right;
            }
            return count;
        }
        private int findChangeNode(TreeNode root, int leftHeight, int rightHeight)
        {
            if (root == null)
                return 0;
            int result = 0;
            int leftRightHeight = findRightLeafHeight(root.left);
            if (leftRightHeight == leftHeight)
                result += (int)Math.Pow(2, leftHeight) - 1;
            else
            {
                result += findChangeNode(root.left, leftHeight - 1, rightHeight - 1);
                return result + (int)Math.Pow(2, rightHeight);
            }
            int rightLeftHeight = findLeftLeafHeight(root.right);
            if (rightLeftHeight == rightHeight)
                result += (int)Math.Pow(2, rightHeight) - 1;
            else
                result += findChangeNode(root.right, leftHeight - 1, rightHeight - 1);
            return result + 1;
        }

        public ListNode MergeTwoLists(ListNode l1, ListNode l2)
        {
            ListNode result = new ListNode(0);
            ListNode TempNode = result;
            while (l1 != null || l2 != null)
            {
                if (l1 == null)
                {
                    TempNode.next = l2;
                    l2 = l2.next;
                }
                else if (l2 == null)
                {
                    TempNode.next = l1;
                    l1 = l1.next;
                }
                else
                {
                    if (l1.val > l2.val)
                    {
                        TempNode.next = l2;
                        l2 = l2.next;
                    }
                    else
                    {
                        TempNode.next = l1;
                        l1 = l1.next;
                    }
                }
                TempNode = TempNode.next;
            }
            return result.next;
        }

        private int lastLayerCount(TreeNode root, int rightHeight)
        {
            int currentHeight = 1;
            int count = 0;
            TreeNode currentNode = root;
            while (currentNode.right != null)
            {
                TreeNode tempNode = currentNode.left;
                for (int i = 1; i <= rightHeight - currentHeight; i++)
                {
                    tempNode = tempNode.left;
                    int leftHeight = findRightLeafHeight(currentNode.left);
                    if (rightHeight - currentHeight == rightHeight)
                        return count;
                    else
                        count += 2;
                }
                currentNode = currentNode.right;
            }
            return 0;
        }

        public int Divide(int dividend, int divisor)
        {
            if (divisor == 0 || (dividend == int.MaxValue && divisor == 1) || (dividend == int.MinValue && divisor == -1))
                return int.MaxValue;
            bool isNagetaiv = false;
            if (dividend < 0)
            {
                dividend = ~dividend + 1;
                isNagetaiv = true;
            }
            if (divisor < 0)
            {
                divisor = ~divisor + 1;
                isNagetaiv = isNagetaiv == false;
            }

            int result = 0;
            int maxdate = 0;
            int temp = divisor;
            for (int i = 0; i < 32; i++)
            {
                if (dividend < temp)
                {
                    maxdate = i;
                    break;
                }
                temp = temp << 1;
            }
            for (int i = 0; i < maxdate; i++)
            {
                result = result << 1;
                temp = temp >> 1;
                if (dividend >= temp)
                {
                    dividend -= temp;
                    result += 1;
                }
            }
            if (isNagetaiv)
                result = ~result + 1;
            return result;
        }

        #region String
        public int StrStr(string haystack, string needle)
        {
            return haystack.IndexOf(needle);
        }

        public class ListNode
        {
            internal ListNode next;
            internal int val;

            public ListNode(int v)
            {
                this.val = v;
            }
        }

        public class TreeNode
        {
            internal TreeNode right;

            public TreeNode left;
        }
        #endregion
    }
}
