const API_BASE = import.meta.env.VITE_API_BASE ?? '';

export interface Area {
  id: number;
  name: string | null;
  width: number;
  height: number;
  parentAreaId: number | null;
  areaTypeId: number | null;
}

export interface AreaType {
  id: number;
  name: string | null;
  definition: string | null;
}

async function getJson<T>(path: string): Promise<T> {
  const res = await fetch(`${API_BASE}${path}`);
  if (!res.ok) throw new Error(`API error ${res.status} on ${path}`);
  return res.json();
}

export const fetchAreas = () => getJson<Area[]>('/api/areas');
export const fetchAreaTypes = () => getJson<AreaType[]>('/api/area-types');
