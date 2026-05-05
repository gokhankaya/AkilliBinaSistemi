import { useQuery } from '@tanstack/react-query';
import { Alert, Loader, Table, Title } from '@mantine/core';
import { fetchAreas } from '../api';

export default function AreasPage() {
  const { data, isLoading, error } = useQuery({ queryKey: ['areas'], queryFn: fetchAreas });

  if (isLoading) return <Loader />;
  if (error) return <Alert color="red">Error: {String(error)}</Alert>;

  return (
    <>
      <Title order={2} mb="md">Areas</Title>
      <Table striped withTableBorder withColumnBorders>
        <Table.Thead>
          <Table.Tr>
            <Table.Th>ID</Table.Th>
            <Table.Th>Name</Table.Th>
            <Table.Th>Width</Table.Th>
            <Table.Th>Height</Table.Th>
            <Table.Th>Parent</Table.Th>
            <Table.Th>Type</Table.Th>
          </Table.Tr>
        </Table.Thead>
        <Table.Tbody>
          {data?.map((a) => (
            <Table.Tr key={a.id}>
              <Table.Td>{a.id}</Table.Td>
              <Table.Td>{a.name ?? '—'}</Table.Td>
              <Table.Td>{a.width}</Table.Td>
              <Table.Td>{a.height}</Table.Td>
              <Table.Td>{a.parentAreaId ?? '—'}</Table.Td>
              <Table.Td>{a.areaTypeId ?? '—'}</Table.Td>
            </Table.Tr>
          ))}
        </Table.Tbody>
      </Table>
    </>
  );
}
