export interface Movie {
  movieId: number;
  title: string;
  description: string | null;
  durationMinutes: number;
  releaseDate: string;
  language: string;
  genre: string;
}

export interface CreateMovieRequest {
  title: string;
  description: string | null;
  durationMinutes: number;
  releaseDate: string;
  language: string;
  genre: string;
}
export interface UpdateMovieRequest {
  title: string;
  description: string | null;
  durationMinutes: number;
  releaseDate: string;
  language: string;
  genre: string;
}
