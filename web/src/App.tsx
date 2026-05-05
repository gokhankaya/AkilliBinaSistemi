import { MantineProvider } from '@mantine/core';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import Layout from './Layout';
import AreasPage from './pages/AreasPage';
import AreaTypesPage from './pages/AreaTypesPage';

import '@mantine/core/styles.css';

const queryClient = new QueryClient();

export default function App() {
  return (
    <MantineProvider>
      <QueryClientProvider client={queryClient}>
        <BrowserRouter>
          <Routes>
            <Route element={<Layout />}>
              <Route index element={<Navigate to="/areas" replace />} />
              <Route path="/areas" element={<AreasPage />} />
              <Route path="/area-types" element={<AreaTypesPage />} />
            </Route>
          </Routes>
        </BrowserRouter>
      </QueryClientProvider>
    </MantineProvider>
  );
}
