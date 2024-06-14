export function getArrayValue<TValue>(
	array: ReadonlyArray<TValue>,
	index: number,
): TValue {
	if (index in array) {
		return array[index];
	}

	throw new Error();
}

export function getRecordValue<TKey extends string | number | symbol, TValue>(
	record: Record<TKey, TValue>,
	key: TKey,
): TValue {
	if (key in record) {
		return record[key];
	}

	throw new Error();
}

export function getMapValue<TKey, TValue>(
	map: Map<TKey, TValue>,
	key: TKey,
): TValue {
	if (map.has(key)) {
		// biome-ignore lint/style/noNonNullAssertion: <explanation>
		const value = map.get(key)!;
		return value;
	}

	throw new Error();
}
