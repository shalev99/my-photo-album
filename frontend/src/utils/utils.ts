export const ALLOWED_FILE_TYPES = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];
export const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB

export const sanitizeInput = (input: string): string => {
  return input.replace(/[<>{}]/g, '');
};

export const handleFileValidation = (file: File | null): string | null => {
  if (!file) return 'No file selected!';

  if (!ALLOWED_FILE_TYPES.includes(file.type)) {
    return 'Invalid file type! Please upload an image (JPEG, PNG, GIF, WebP).';
  }

  if (file.size > MAX_FILE_SIZE) {
    return 'File size exceeds 5MB! Please upload a smaller image.';
  }

  return null;
};
