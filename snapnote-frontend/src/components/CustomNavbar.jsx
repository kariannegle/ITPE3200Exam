import React from 'react';
import { Navbar, Nav, Container, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const CustomNavbar = ({ isAuthenticated }) => {
  return (
    <Navbar expand="md" bg="light" variant="light" fixed="top" className="navbar">
      <Container fluid>
        <Button
          variant="outline-secondary"
          className="me-2 d-md-none"
          onClick={() => console.log('Sidebar toggle clicked')}
        >
          <i className="fas fa-bars"></i>
        </Button>

        <Navbar.Brand as={Link} to="/">SnapNote</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="ms-auto">
            <Nav.Link as={Link} to="/">Home</Nav.Link>
            <Nav.Link as={Link} to="/posts">Posts</Nav.Link>
            {isAuthenticated ? (
              <>
                <Nav.Link as={Link} to="/logout">Logout</Nav.Link>
              </>
            ) : (
              <>
                <Nav.Link as={Link} to="/login">Login</Nav.Link>
                <Nav.Link as={Link} to="/register">Register</Nav.Link>
              </>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default CustomNavbar;
