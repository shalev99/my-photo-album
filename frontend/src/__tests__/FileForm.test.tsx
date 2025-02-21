import { render, screen, fireEvent } from '@testing-library/react';
import FileForm from '../components/fileForm';
import { useFilesStore } from '../store/usefilesStore';
import '@testing-library/jest-dom';

// Create a mock for Zustand's useFilesStore
jest.mock('../store/usefilesStore', () => ({
  useFilesStore: jest.fn(),
}));

describe('FileForm Component', () => {
  beforeEach(() => {
    // Cast to unknown first, then to jest.Mock
    (useFilesStore as unknown as jest.Mock).mockReturnValue({
      addFile: jest.fn(),
    });
  });

  test('renders FileForm', () => {
    render(<FileForm />);
    expect(screen.getByText(/Picture Name:/)).toBeInTheDocument();
    expect(screen.getByText(/Add Picture/)).toBeInTheDocument();
  });

  test('allows user to enter file details', () => {
    render(<FileForm />);

    const nameInput = screen.getByLabelText(/Picture Name:/);
    fireEvent.change(nameInput, { target: { value: 'Test Image' } });

    expect(nameInput).toHaveValue('Test Image');
  });

  test('displays error when submitting without file', async () => {
    render(<FileForm />);

    const submitButton = screen.getByText(/Add Picture/);
    fireEvent.click(submitButton);

    expect(await screen.findByText(/File name and image file are required!/)).toBeInTheDocument();
  });
});
