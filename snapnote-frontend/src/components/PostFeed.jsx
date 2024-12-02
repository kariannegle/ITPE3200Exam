import React from 'react';
import PropTypes from 'prop-types';
import Post from './Post.jsx'; // Ensure the path is correct

const PostFeed = ({ posts, onDelete }) => {
  console.log("PostFeed received onDelete:", typeof onDelete);

  if (typeof onDelete !== 'function') {
    console.error("The onDelete function is missing in PostFeed!");
    return null; // Optionally, render a fallback UI
  }

  return (
    <div className="post-feed">
      {posts && posts.length > 0 ? (
        posts.map(post => (
          <Post key={post.id} post={post} onDelete={onDelete} />
        ))
      ) : (
        <p>No posts available.</p>
      )}
    </div>
  );
};

PostFeed.propTypes = {
  posts: PropTypes.arrayOf(PropTypes.object).isRequired,
  onDelete: PropTypes.func.isRequired,
};

export default PostFeed;
