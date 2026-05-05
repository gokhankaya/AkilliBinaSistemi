import { useQuery } from '@tanstack/react-query';
import { Alert, Loader, Table, Title } from '@mantine/core';
import { fetchAreaTypes } from '../api';

export default function AreaTypesPage() {
  const { data, isLoading, error } = useQuery({ queryKey: ['area-types'], queryFn: fetchAreaTypes });

  if (isLoading) return <Loader />;
  if (error) return <Alert color="red">Error: {String(error)}</Alert>;

  return (
    <>
      <Title order={2} mb="md">Area Types</Title>
      <Table striped withTableBorder withColumnBorders>
        <Table.Thead>
          <Table.Tr>
            <Table.Th>ID</Table.Th>
            <Table.Th>Name</Table.Th>
            <Table.Th>Definition</Table.Th>
          </Table.Tr>
        </Table.Thead>
        <Table.Tbody>
          {data?.map((t) => (
            <Table.Tr key={t.id}>
              <Table.Td>{t.id}</Table.Td>
              <Table.Td>{t.name ?? '—'}</Table.Td>
              <Table.Td>{t.definition ?? '—'}</Table.Td>
            </Table.Tr>
          ))}
        </Table.Tbody>
      </Table>
    </>
  );
}
