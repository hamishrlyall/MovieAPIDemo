import './App.css';
import { Container, Navbar, Nav } from 'react-bootstrap';
import { BrowserRouter, Link, Route, Routes } from 'react-router-dom';
import Landing from './pages/landing';
import Actors from './pages/actors';
import EditMovie from './components/edit-movie';
import MovieDetail from './components/movie-details';
import CreateEditActor from './components/create-edit-actor';
import ActorDetail from './components/actor-details';

function App() {
  return (
    <Container>
      <BrowserRouter>
        <Navbar bg="dark" variant="dark">
          <Navbar.Brand as={Link} to="/">Movie World</Navbar.Brand>
          <Nav className="mr-auto">
            <Nav.Link as={Link} to="/movies">Movies</Nav.Link>
            <Nav.Link as={Link} to="/actors">Actors</Nav.Link>
          </Nav>
        </Navbar>
        <Routes>
          <Route path="/" element={<Landing/>} />
          <Route path="/details/:movieid" element={<MovieDetail/>} />
          <Route path="/edit/:movieid" element={<EditMovie/>} />
          <Route path="/actors" element={<Actors/>} />
          <Route path="/actors/create-edit/:personid" element={<CreateEditActor/>} />
          <Route path="/actors/details/:actorid" element={<ActorDetail/>} />
        </Routes>
      </BrowserRouter>
    </Container>
  );
}

export default App;
