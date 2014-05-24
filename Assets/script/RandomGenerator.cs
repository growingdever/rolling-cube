using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomGenerator {

	List<Range> _rangeList;

	public RandomGenerator() {
		_rangeList = new List<Range> ();
	}

	public void AddRange(Range range) {
		_rangeList.Add (range);
	}

	public float Next() {
		int selectRange = Random.Range(0, _rangeList.Count);
		Range range = _rangeList [selectRange];
		return Random.Range (range._start, range._end);
	}
	public int NextInt() {
		int selectRange = Random.Range(0, _rangeList.Count);
		Range range = _rangeList [selectRange];
		return Random.Range ((int)range._start, (int)range._end);
	}

	public class Range {
		public float _start, _end;

		public Range(float start, float end) {
			_start = start;
			_end = end;
		}
	}
}
