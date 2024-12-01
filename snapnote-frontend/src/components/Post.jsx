import React from 'react';

const Post = ({ post }) => {
  return (
    <div className="post">
      <div className="post-details-box">
        <div className="userinfo">
          <div className="userImg">
            <img src="default-user.png" alt="User" />
          </div>
          <span className="username">{post.author}</span>
        </div>
        <span className="post-date">{post.date}</span>
      </div>
      {post.image && (
        <div className="imagecontent">
          <img src={URL.createObjectURL(post.image)} alt="Post Content" />
        </div>
      )}
      <div className="notecontentwithoutimage">
        <p>{post.content}</p>
      </div>
    </div>
  );
};

export default Post;
