using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PQueue {

	public class PNode
	{
		public Waypoint point;
		public float cost;
		public PNode next;
		public PNode prev;
		public PNode(Waypoint w, float cost) {
			this.cost = cost;
			point = w;
			next = null;
			prev = null;
		}
	}

	public PNode head;
	public PNode tail;
	List<PNode> allNodes;
	int count;

	public PQueue() {
		head = null;
		tail = null;
		allNodes = new List<PNode> ();
		count = 0;
	}

	public void push(Waypoint w, float cost) {
		PNode node = new PNode (w, cost);
		if (count == 0) {
			head = node;
			tail = node;
		} else {
			// find the place to insert
			PNode curr = head;
			while (curr != null && curr.cost < node.cost) {
				curr = curr.next;
			}
			if (curr == null) {
				// we are inserting at the end
				tail.next = node;
				node.prev = tail;
				node.next = null;
				tail = node;
			} else if (curr == head) {
				// we are insterint at the beginning
				head.prev = node;
				node.next = head;
				node.prev = null;
				head = node;
			} else {
				// somewhere in the middle
				curr.prev.next = node;
				node.prev = curr.prev;
				node.next = curr;
				curr.prev = node;
			}

		}
		count++;
	}

	public Waypoint pop() {
		if (head == null)
			return null;
		Waypoint result = head.point;
		if (head == tail)
			tail = null;
		head = head.next;
		count--;
		return result;
	}

	public bool empty() {
		return count == 0;
	}
}
