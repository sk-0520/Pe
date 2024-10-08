select
	KeyActions.KeyActionId,
	KeyMappings.Sequence,
	KeyMappings.Key,
	KeyMappings.Shift,
	KeyMappings.Control,
	KeyMappings.Alt,
	KeyMappings.Super
from
	KeyActions
	inner join
		KeyMappings
		on
		(
			KeyActions.KeyActionId = KeyMappings.KeyActionId
		)
where
	KeyActions.KeyActionKind = @KeyActionKind
	and
	KeyActions.KeyActionContent = @KeyActionContent
order by
	KeyActions.UsageCount desc,
	KeyMappings.Sequence asc
