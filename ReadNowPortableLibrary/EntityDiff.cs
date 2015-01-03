using System.Collections.Generic;
using System.Linq;

namespace ReadNow.Portable
{
	public class EntityDiff
	{
		public bool HasChanges<T, E>(IEnumerable<T> t, IEnumerable<E> e) where T : PocketItem where E : PocketItem
		{
			int tcount = t.Count();
			int ecount = e.Count();

			if (tcount != ecount)
			{
				return true;
			}

			foreach (T current in t)
			{
				E defaultE = e.FirstOrDefault(arg => arg.Id == current.Id);

				if (defaultE == null)
				{
					return true;
				}
			}

			return false;
		}

	}
}