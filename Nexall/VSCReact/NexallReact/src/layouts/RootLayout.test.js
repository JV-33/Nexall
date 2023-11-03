import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import RootLayout from './RootLayout';

describe('RootLayout', () => {
    it('renders without crashing', () => {
        render(<RootLayout />, { wrapper: MemoryRouter });
    });

    it('renders correct nav links', () => {
        render(<RootLayout />, { wrapper: MemoryRouter });
        expect(screen.getByText('Home')).toBeInTheDocument();
        expect(screen.getByText('Saraksts')).toBeInTheDocument();
        expect(screen.getByText('Filter')).toBeInTheDocument();
        expect(screen.getByText('Day Stats')).toBeInTheDocument();
    });
});
