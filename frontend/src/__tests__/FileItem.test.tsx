import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import FileItem from '../components/FileItem';

const mockFileWithContent = {
  id: '124',
  name: 'test-image.jpg',
  description: 'Test image description',
  fileName: 'test-image.jpg',
  fileSize: 1024,
  fileType: 'image/jpeg',
  fileContent: 'sampleBase64Content',
  fileContentBase64: 'sampleBase64Content',
  uploadDate: '2023-01-01',
};

test('renders the file name and id correctly', () => {
  render(<FileItem file={mockFileWithContent} />);
  expect(screen.getByText(mockFileWithContent.name)).toBeInTheDocument();
  expect(screen.getByText(`ID: ${mockFileWithContent.id}`)).toBeInTheDocument();
});

test('renders the image if fileContentBase64 is provided', () => {
  render(<FileItem file={mockFileWithContent} />);
  const image = screen.getByRole('img');
  expect(image).toHaveAttribute('src', `data:${mockFileWithContent.fileType};base64,${mockFileWithContent.fileContentBase64}`);
});