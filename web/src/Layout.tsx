import { AppShell, Group, NavLink, Title } from '@mantine/core';
import { Outlet, NavLink as RouterNavLink } from 'react-router-dom';

export default function Layout() {
  return (
    <AppShell
      header={{ height: 56 }}
      navbar={{ width: 220, breakpoint: 'sm' }}
      padding="md"
    >
      <AppShell.Header>
        <Group h="100%" px="md">
          <Title order={3}>ADLE</Title>
        </Group>
      </AppShell.Header>

      <AppShell.Navbar p="xs">
        <NavLink component={RouterNavLink} to="/areas" label="Areas" />
        <NavLink component={RouterNavLink} to="/area-types" label="Area Types" />
      </AppShell.Navbar>

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>
    </AppShell>
  );
}
